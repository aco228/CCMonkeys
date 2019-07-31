class Socket{

  get host() { return '[HOST]'; }
  get sguid() { return '[SGUID]'; }
  get events() { return{[EVENTS]} }

  constructor(){
    this.callbacks = [];
    this.data = null;

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

    this.console("onMessage", "message received", response.Event, response.Data);

    if(this.callbacks.hasOwnProperty('event_' + response.Event) && typeof this.callbacks['event_' + response.Event] === "function")
      this.callbacks['event_' + response.Event](response.Data);
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

    this.callbacks['event_' + event] = func;
  }

  onError(e){
    this.console("onError", "error", e);
  }

  send(key, data){
    this.console('sendingData', 'beforesend', {key:key, data:data });
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
    for(var i = 0; i <= this.data.actions.length; i++)
      if(this.data.actions[i]==actionID)
        return true;
    return false;
  }

  getCountry(countryid){
    for(var i = 0; i <= this.data.countries.length; i++)
      if(this.data.countries[i].ID==countryid)
        return this.data.countries[i];
    return null;
  }

  getProvider(id){
    for(var i = 0; i <= this.data.providers.length; i++)
      if(this.data.providers[i].ID==id)
        return this.data.providers[i];
    return null;    
  }

  getLander(id){    
    for(var i = 0; i <= this.data.landers.length; i++)
      if(this.data.landers[i].ID==id)
        return this.data.landers[i];
    return null;  
  }

  getPrelander(id){    
    for(var i = 0; i <= this.data.prelanders.length; i++)
      if(this.data.prelanders[i].ID==id)
        return this.data.prelanders[i];
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
        if(tag.isQuestion)
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
      window.Socket.data.actions  = data.Actions;
      window.Socket.data.landers  = data.Landers;
      window.Socket.data.landerTypes  = data.LanderTypes;
      window.Socket.data.prelanders  = data.Prelanders;
      window.Socket.data.prelanderTypes  = data.PrelanderTypes;
      window.Socket.data.countries  = data.Countries;
      window.Socket.data.providers  = data.Providers;

      var ms = ((new Date()).getTime() - window.Socket.data.created.getTime());
      window.Socket.console(`Init complete`, `We got response in ${ms}ms`);
    }
    catch(e){
      console.error('SOCKET::Data. We got exception on initialization', e);
    }
  }

  onActionConnect(data){
    var self = window.Socket.data;
    for(var i = 0; i <= self.actions.length; i++)
      if(self.actions[i]==data.ID)
        return;
    window.Socket.console('+', 'New action has connected with id: ' + data.ID);
    self.actions.push(data.ID);
  }

  onActionDisconnect(data){
    var self = window.Socket.data;
    for(var i = 0; i <= self.actions.length; i++)
      if(self.actions[i]==data.ID)
      {
        self.actions.slice(i, 1);
        window.Socket.console('-', 'Action has disconnected with id: ' + data.ID);
        return;
      }
  }
}


window.Socket = new Socket();
var socket = window.Socket;