class Tag
{
  constructor(isQuestion, name, value, answers){ 
    this.isQuestion = isQuestion;
    this.name = name;
    this.value = value;
    this.answers = answers;
  }
}

document.addEventListener('DOMContentLoaded', function(){
  if(typeof CC === 'undefined'){
    alert('no CC wtf??');
    return;
  }
  CC.type = 'pl';
  CC.host = 'localhost:5001';
  CC.dbg=true;
  CC.steps = 0;
  CC.tags = [];

  pT(); pQ();
  console.log(CC.tags);

  CC.api.init({
    success:function(msg, e) { 
      CC.api.send('pl-init', { tags: CC.tags });
    },
    error: function(e) { console.log('init error'); }
  });

  //
  // Collect all tags
  //

  function pT(){
    try
    {
      var ts = document.querySelectorAll('[cctag]');
      ts.forEach((t) => {
        CC.steps++;
        t.addEventListener('click', (e) => {
          CC.api.send('pl-tag', { tag:t.getAttribute('cctag') });
        }, false);
        CC.tags.push(new Tag(false, t.getAttribute('cctag'), '',  null));
      });
    }
    catch{}
  }

  //
  // Collect all questions
  //

  function pQ(){
    try
    {
      var qs = document.getElementsByClassName('ccq');
      var index = 0;
      qs.forEach((q) =>{
        CC.steps++;
        let qTxt = q.getElementsByClassName('ccqt')[0].textContent;
        var tag = new Tag(true, 'q' + index, qTxt, []);
        var aIndex = 0;

        q.getElementsByClassName('ccqa').forEach((a) => {
          
          // setting attributes for question and answer
          a.setAttribute('ccq', index);
          a.setAttribute('ccqa', aIndex);
          a.addEventListener('click', (e)=>{
            CC.api.send('pl-q', 
            { 
              tag: 'q'+a.getAttribute('ccq'), 
              answer:'ccqa'+a.getAttribute('ccqa')  
            });
            
          }, false);
          tag.answers.push(a.textContent);
          aIndex++;
        });

        CC.tags.push(tag);
        index++;
      });
    }
    catch(e){ console.error(e);}
  };

}, false);

HTMLCollection.prototype.forEach = Array.prototype.forEach;
HTMLCollection.prototype.getElementsByClassName = function( name ){
  var all = [];
  this.forEach( function( el ){
    if(el) all.concat( el.getElementsByClassName( name ) );
  });
  return all;
}