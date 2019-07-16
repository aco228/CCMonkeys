document.addEventListener('DOMContentLoaded', function(){
  if(typeof CC === 'undefined'){
    alert('no CC wtf??');
    return;
  }

  CC.api.init({
    success:function(msg, e){
      console.log('init success', msg, e);
    },
    error: function(e){
      // REDIRECTION LOCGIC OR SOMETHING (SERVER IS DOWN or connection is unstable)
      console.log('init error');
    }
  });

}, false);