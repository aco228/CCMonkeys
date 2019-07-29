CC.host = '';
CC.dbg = false;
CC.type = 'lp';
CC.api = {
  callbacks: [],
  socket: null,
  onopen: null,
  onclose: null,
  onmessage: null,
  onerror: null,
  created: null,

  init: function(registration_callback, providerID){

    if(CC.host == ''){
      console.error('host is not defined!!');
      return;
    }

    if(this.checkCallback(registration_callback)){
      console.error('registration_callback missing or misformed!! concentrate!');
      return;
    }

    var self = this;
    self.created = new Date();
    var registered = false;
    if(CC.host.indexOf('wss://') == -1)
      CC.host = 'wss://' + CC.host;
    this.socket = new WebSocket(CC.host + '/ws_api?type=' + CC.type);
    
    this.socket.onopen = e => {
      self.console('onopen', e);
      if(!registered){
        self.send('register', { url:document.location.href, providerID: (typeof providerID === 'undefined' ? null : providerID) }, registration_callback);
        registered = true;
      }

      if(typeof self.onopen === 'function') self.onopen(e);
    };
    this.socket.onclose = function (e) {
      if(typeof self.onclose === 'function') self.onclose(e);
      registration_callback.error(e);
      self.console('onclose', e);
    };

    this.socket.onmessage = function (e) {
      if(typeof self.onmessage === 'function') self.onmessage(e);
      var response = JSON.parse(e.data);
      var milisecondsPassed = 0;

      for(var i = 0; i < self.callbacks.length; i++)
        if(self.callbacks[i].Key === response.key)
        {
          if(response.Status) {
            self.callbacks[i].success(response.Data, response.Message);
          }
          else {
            self.callbacks[i].error(response.Data, response.Message);
          }
          milisecondsPassed = ((new Date()).getTime() - self.callbacks[i].created.getTime());
          self.callbacks.splice(i, 1);
          break;
        }

      if(response.Key === 'reg-post'){
        milisecondsPassed = ((new Date()).getTime() - self.created.getTime());
      }

      self.console(`We got response in ${milisecondsPassed} miliseconds for #${response.Key}!`, response);
    };

    this.socket.onerror = function (e) {
      if(typeof self.onerror === 'function') self.onerror(e);
      registration_callback.error(e);
      self.console('onerror', e);
    };

  },

  send: function(key, data, callback){
    if(typeof callback === 'undefined') {
      callback = { success:function(){}, error:function(){} };
      this.console(`creating empty callback for #${key}`);
    }
    if(typeof callback !== 'undefined' && this.checkCallback(callback)){
      this.console("error with callback. callback must be object with 'success' and 'error' functions!!!!");
      return;
    }

    callback.created = new Date();
    callback.key = key;
    this.callbacks.push(callback);

    this.console('sendingData', {key:key, data:data });
    this.socket.send(key + '#' + JSON.stringify(data));
  },

  checkCallback: function(callback){
    if(typeof callback !== "object" || typeof callback.success !== "function" || typeof callback.error !== "function")
      return true;
    return false;
  },

  console: function(data, obj){
    if(!CC.dbg) return;
    console.warn('ccsocket:: ' + data, obj);
  }

};