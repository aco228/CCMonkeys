class Socket{

  get host() { return '[HOST]'; }
  get sguid() { return '[SGUID]'; }
  get events() { return{"[EVENTS]"} }

  constructor(){
    this.callbacks = [];
    this.data = null;
    this.onready = null;

    this.registered = false;
    this.wsHost = `${this.host}/ws_dashboard?sguid=${this.sguid}`;
    this.socket = new WebSocket(this.wsHost);
    this.socket.onopen = e => this.onOpen(e);
    this.socket.onclose = e => this.onClose(e);
    this.socket.onmessage = e => this.onMessage(e);
    this.socket.onerror = e => this.onError(e);
  }

  onOpen(e){
    this.console("onOpen", "connection opened", e);
    if(!this.registered){
      this.data = new SocketData(this);
      this.send('register', {sguid:this.sguid});
      this.register = true;
    }
  }

  onClose(e){
    this.console("onClose", "connection closed", e);
  }

  onMessage(e){
    var response = JSON.parse(e.data);

    // in case of direct we will have to double covert it
    if(typeof response == 'string')
      response = JSON.parse(response); 

    // in case of DIRECT db MESSAGE
    if(typeof response.IsDirect !== 'undefined') {
      if(typeof window.Direct !== 'undefined')
        window.Direct.onMessage(response);
      return;
    }

    if(this.callbacks.hasOwnProperty('event_' + response.Event))
    {
      for(var i = 0; i < this.callbacks['event_' + response.Event].length; i++)
        this.callbacks['event_' + response.Event][i](response.Data);
    }
  }

  subscribe(event, func){
    if(typeof event !== 'number'){
      console.error('socket:: subscribe must have number event representation');
      return;
    }
    if(typeof func !== 'function'){
      console.error('socket:: subscribe must have function callback');
      return;
    }

    if(typeof this.callbacks['event_' + event] === 'undefined')
      this.callbacks['event_' + event] = [];

    this.callbacks['event_' + event].push(func);
  }

  onError(e){
    this.console("onError", "error", e);
  }

  send(key, data){
    //this.console('sendingData', 'beforesend', {key:key, data:data });
    this.socket.send(key + '#' + JSON.stringify(data));
  }

  console(action, ...args){
    var cssRule =
      "color: rgb(0, 0, 0); background: #eff1e8; font-size: 14px; font-weight: bold; text-shadow: 1px 1px 5px rgb(249, 162, 34); filter: dropshadow(color=rgb(249, 162, 34), offx=1, offy=1);";
    console.info("%c socket::" + action, cssRule, args);
  }

  //
  // HELPERS
  //

  isActionLive(actionID){
    return this.data.actions.hasOwnProperty(actionID);
  }

  getCountry(id){
    if(typeof this.data.countries[id] !== 'undefined')
      return this.data.countries[id];
    return null;
  }

  getProvider(id){
    if(typeof this.data.providers[id] !== 'undefined')
      return this.data.providers[id];
    return null;
  }

  getLander(id){    
    if(typeof this.data.landers[id] !== 'undefined')
      return this.data.landers[id];
    return null;
  }

  getPrelander(id){    
    if(typeof this.data.prelanders[id] !== 'undefined')
      return this.data.prelanders[id];
    return null;
  }

  ///
  /// PRELANDER DAT
  ///

  ___getPrelanderTag(prelander, tagName){
    try{
      var result = {};
      if(prelander == null || typeof prelander.Tags === 'undefined' || prelander.Tags == null) return null;
      for(var i = 0; i < prelander.Tags.length; i++)
        if(prelander.Tags[i].name == tagName){
          result.tag = prelander.Tags[i];
          break;
        }
      if(typeof result.tag !== 'undefined' && result.tag != null && result.tag.isQuestion && typeof prelander.Answers !== 'undefined' && prelander.Answers.length > 0){
        result.answers = [];
        for(var i = 0; i < prelander.Answers.length; i++)
          if(prelander.Answers[i].tagName == tagName)
            result.answers.push(prelander.Answers[i].value);
      }
      else  
        result.answers = null;
      return result;
    }
    catch(exception){
      console.error("socket.getPrelanderTag PRELANDER MUST BE prelander object");
    }
  }

  getPrelanderTags(prelanderid, prelanderData){
    try{
      var split = prelanderData.split('.');
      var prelander = this.getPrelander(prelanderid);
      var result = [];
      split.forEach((s)=>{
        if(s == '') return;
        var info = s.split('=');
        if(info.length != 2) return;

        var tagResponse = this.___getPrelanderTag(prelander, info[0]);
        if(tagResponse == null || typeof tagResponse.tag === 'undefined' || tagResponse.tag == null)
          return null;

        var tag = tagResponse.tag;
        if(tag.isQuestion && info[1] != '')
        {
          var num = parseInt(info[1]);
          if(typeof tagResponse.answers !== 'undefined' && tagResponse.answers != null && num <= tagResponse.answers.length)
            tag.answer = tagResponse.answers[num];
        }

        result.push(tag);
      });
      return result;
    }
    catch(exception){
      console.error('getPrelanderTags has fatal exception', exception);
    }
  }
}


//
// ---------------------------------------------------------------------------------------------
// DATA OBJ
//

class SocketData{

  get activeActions(){ return this.actions.length; }

  constructor(socket){
    this.actions = [];
    this.landers = [];
    this.landerTypes = [];
    this.prelanders = [];
    this.prelanderTypes = [];
    this.countries = [];
    this.providers = [];
    this.created = new Date();

    socket.subscribe(1, this.onInit);
    socket.subscribe(2, this.onActionConnect);
    socket.subscribe(3, this.onActionDisconnect);
  }

  onInit(data){
    try{
      window.Socket.data.actions = window.Socket.data.mapActions(window.Socket.data.actions, data.Actions);
      window.Socket.data.landers = socket.data.mapValues(window.Socket.data.landers, data.Landers);
      window.Socket.data.landerTypes = socket.data.mapValues(window.Socket.data.landerTypes, data.LanderTypes);
      window.Socket.data.prelanders = socket.data.mapValues(window.Socket.data.prelanders, data.Prelanders);
      window.Socket.data.prelanderTypes = socket.data.mapValues(window.Socket.data.prelanderTypes, data.PrelanderTypes);
      window.Socket.data.countries = socket.data.mapValues(window.Socket.data.countries, data.Countries);
      window.Socket.data.providers = socket.data.mapValues(window.Socket.data.providers, data.Providers);

      var ms = ((new Date()).getTime() - window.Socket.data.created.getTime());
      window.Socket.console(`Init complete`, `We got response in ${ms}ms`);

      if(typeof window.Socket.onready === 'function')
        window.Socket.onready();
    }
    catch(e){
      console.error('SOCKET::Data. We got exception on initialization', e);
    }
  }

  onActionConnect(data){
    var self = window.Socket.data;
    if(!window.Socket.data.actions.hasOwnProperty(data))
      window.Socket.data.actions[data] = true;
  }

  onActionDisconnect(data){
    var self = window.Socket.data;
    if(self.actions.hasOwnProperty(data)){
      self.actions[data] = false;
      delete self.actions[data];
    }
  }

  mapActions(input, data){
    if(typeof data === 'object')
      for(var i = 0; i < data.length; i++)
        if(!input.hasOwnProperty(data[i]))
          input[data[i]] = true;
    return input;
  }

  mapValues(input, data){
    for(var i = 0; i < data.length; i++)
      if(!input.hasOwnProperty(i.ID))
      {
        input[data[i].ID] = data[i];
      }
    return input;
  }

}


window.Socket = new Socket();
var socket = window.Socket;