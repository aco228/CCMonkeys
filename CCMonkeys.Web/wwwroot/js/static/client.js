CC.host = 'wss://localhost:5001';
CC.dbg = true;
CC.type = 'lp';
CC.connected = false;
CC.useBackup = false;
CC.backup = { hasValue: false };
CC.api = {
  callbacks: [],
  socket: null,
  onopen: null,
  onclose: null,
  onregister: null,
  onregpost: null,
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
    this.socket = new WebSocket(CC.host + '/ws_api?type=' + CC.type);
    
    this.socket.onopen = e => {
      self.console('onopen', e);
      if(!registered){
        self.send('register', { url:document.location.href, providerID: (typeof providerID === 'undefined' ? null : providerID) }, registration_callback);
        registered = true;
      }

      CC.connected = true;
      if(typeof self.onopen === 'function') self.onopen(e);
    };
    this.socket.onclose = function (e) {
      if(typeof self.onclose === 'function') self.onclose(e);
      registration_callback.error(e);
      self.console('onclose', e);
      CC.connected = false;
      CC.useBackup = true;
      CC.api.socket = null; // disable socket for further use!!

      for(var i = 0; i < CC.api.callbacks.length; i++)
        CC.api.ajax(CC.api.callbacks[i].key, CC.api.callbacks[i].data, CC.api.callbacks[i]);
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

      if(response.Key == 'register' && typeof CC.api.onregister === 'function')
        CC.api.onregister(response);

      if(response.Key === 'reg-post'){
        milisecondsPassed = ((new Date()).getTime() - self.created.getTime());
        CC.backup.hasValue = true;
        if(typeof response.Data.actionID !== 'undefined')
          CC.backup.actionid = response.Data.actionID;
        if(typeof response.Data.sessionID !== 'undefined')
          CC.backup.sessionid = response.Data.sessionID;
        if(typeof response.Data.userID !== 'undefined')
          CC.backup.userid = response.Data.userID;

        if(typeof CC.api.onregpost === 'function')
          CC.api.onregpost(response);
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
    callback.data = data;
    this.callbacks.push(callback);

    this.console('sendingData', {key:key, data:data });

    if(!CC.useBackup)
      this.socket.send(key + '#' + JSON.stringify(data));
    else
      this.ajax(key, data, callback);
  },

  ajax: function(key, data, callback){
    var parsed = key + '|' + JSON.stringify(data);
    var xmlhttp = new XMLHttpRequest();
    console.log(data);
    var url = CC.host.replace('wss://', 'https://') + '/sb/' + CC.type + '/' + parsed;
    console.log(url);

    xmlhttp.withCredentials = true;
    xmlhttp.onreadystatechange = function() {
      if (xmlhttp.readyState == XMLHttpRequest.DONE) {   // XMLHttpRequest.DONE == 4
        if (xmlhttp.status == 200) {
          var response = JSON.parse(xmlhttp.responseText);
          console.log(response);
          callback.success(response);
        }
        else
          callback.error(null);
      }
    };

    xmlhttp.open("GET", url, true);
    xmlhttp.send();
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