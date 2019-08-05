class Tag
{
  constructor(isQuestion, name, value, answers){ 
    this.isQuestion = isQuestion;
    this.name = name;
    this.value = value;
    this.answers = answers;
  }
}


HTMLCollection.prototype.forEach = Array.prototype.forEach;
HTMLCollection.prototype.getElementsByClassName = function (name) {
  var all = [];
  this.forEach(function (el) {
    if (el) all.concat(el.getElementsByClassName(name));
  });
  return all;
}

CC.isInitiated = false;
CC.prelanderCache = [];
CC.psend = function(key, data){
  if(!CC.isInitiated){
    console.log('initiate is not ready');
    CC.prelanderCache.push({key:key, data:data});
    return;
  }

  CC.api.send(key, data);

  if(CC.prelanderCache != null && CC.prelanderCache.length != 0){
    console.log('sending ' + CC.prelanderCache.length + ' caches data');
    for(var i =0; i < CC.prelanderCache.length; i++)
      CC.api.send(CC.prelanderCache[i].key, CC.prelanderCache[i].data);
    CC.prelanderCache = null;
  }
}

document.addEventListener('DOMContentLoaded', function(){
  if(typeof CC === 'undefined'){
    alert('no CC wtf??');
    return;
  }
  CC.type = 'pl';
  CC.steps = 0;
  CC.tags = [];
  CC.prelanderID = null;

  CC.pT = function(){
    try
    {

      //
      // Collect all tags
      //

      var ts = document.querySelectorAll('[cctag]');
      ts.forEach((t) => {
        CC.steps++;
        t.addEventListener('click', (e) => {
          CC.psend('pl-tag', { prelanderid: CC.prelanderID,  tag:t.getAttribute('cctag') });
        }, false);
        CC.tags.push(new Tag(false, t.getAttribute('cctag'), '',  null));
      });

      //
      // Collect all questions
      //

      var qs = document.getElementsByClassName('ccq');
      var index = 0;
      qs.forEach((q) =>{
        CC.steps++;
        let qTxt = q.getElementsByClassName('ccqt')[0].textContent;
        var tag = new Tag(true, 'q' + index, qTxt, []); // tag
        var aIndex = 0; // answerIndex

        q.getElementsByClassName('ccqa').forEach((a) => {
          
          // setting attributes for question and answer
          a.setAttribute('ccq', index);
          a.setAttribute('ccqa', aIndex);
          a.addEventListener('click', (e)=>{
            CC.psend('pl-q', 
            { 
              prelanderid: CC.prelanderID, 
              tag: 'q'+a.getAttribute('ccq'), 
              answer:'ccqa'+a.getAttribute('ccqa'),
              index: a.getAttribute('ccqa')
            });
            
          }, false);
          tag.answers.push(a.textContent);
          aIndex++;
        });

        CC.tags.push(tag);
        index++;
      });

    }
    catch(e){console.error(e);  }
  }


  
  console.log(CC.tags);
  CC.pT(); 

  CC.api.init({
    success:function(msg, e) {},
    error: function(e) { console.log('init error'); }
  });

  CC.api.onregister = function(msg){
    CC.prelanderID = msg.prelanderID;
    CC.isInitiated = true;
    CC.psend('pl-init', { prelanderid: CC.prelanderID, tags: CC.tags });
    console.log('prelander:: ',msg);
  };

  CC.api.onregpost = function(msg){
    document.body.append(msg.Data.actionID);
    console.log('onregpost', msg);
  }


}, false);
