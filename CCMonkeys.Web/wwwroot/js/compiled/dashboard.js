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
  // HELPERS dada
  //

  actionsCount() { return this.data.actionsCount; }
  adminsLive(){ return this.data.dashboardSessionsCount; }

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
      var tags = prelander.Tags;

      for(var i = 0; i < tags.length; i++)
        if(tags[i].name == tagName){
          result.tag = tags[i];
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
      return JSON.parse( JSON.stringify( result ) );
    }
    catch(exception){
      console.error("socket.getPrelanderTag PRELANDER MUST BE prelander object");
    }
  }

  getPrelanderTags(prelanderid, prelanderData){
    try{
      var split = prelanderData.split('.');
      var prelander = this.getPrelander(prelanderid);
      //console.log(prelander);

      var result = {
        count:0,
        withValues:0,
        data:[]
      };

      split.forEach((s)=>{
        if(s == '') return;
        var info = s.split('=');
        //console.log('info', info);
        if(info.length != 2) return;

        var tagResponse = this.___getPrelanderTag(prelander, info[0]);
        result.count++;
        if(info[1] != '')
          result.withValues++;

        //console.log('tagResponse', tagResponse);
        if(tagResponse == null || typeof tagResponse.tag === 'undefined' || tagResponse.tag == null)
          return null;

        var tag = tagResponse.tag;
        tag.hasValue = false;

        if(info[1] != '')
        {
          tag.hasValue = true;
          if(tag.isQuestion)
          {
            var num = parseInt(info[1]);
            if(typeof tagResponse.answers !== 'undefined' && tagResponse.answers != null && num <= tagResponse.answers.length)
              tag.answer = tagResponse.answers[num];
          }

        }

        result.data.push(tag);
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

  get activeActions(){ return this.actionsCount; }

  constructor(socket){
    this.dbg = false;
    this.actions = [];
    this.actionsCount = 0;
    this.dashboardSessions = [];
    this.dashboardSessionsCount = 0;
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

    socket.subscribe(4, this.onAdminConnect);
    socket.subscribe(5, this.onAdminDisconnect);
  }

  onInit(data){
    try{

      // check if we are in debug enviorement
      let url = new URL(window.location.href);
      if(url.searchParams.get('dbg'))
        socket.data.dbg = true;

      window.Socket.data.actions = window.Socket.data.mapActions(window.Socket.data.actions, data.Actions);
      window.Socket.data.actionsCount = data.Actions.length;
      window.Socket.data.dashboardSessions = data.DashboardSessions;
      window.Socket.data.dashboardSessionsCount = data.DashboardSessions.length;
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

  /*
    ACTION 
  */

  onActionConnect(data){
    var self = window.Socket.data;
    if(!window.Socket.data.actions.hasOwnProperty(data.ID)){
      window.Socket.data.actions[data.ID] = true;
      if(self.dbg) console.log('onActionConnect', data);
      window.Socket.data.actionsCount++;
    }
    else {
      if(self.dbg) console.log('onActionConnect (there were some)', data);
    }

  }

  onActionDisconnect(data){
    var self = window.Socket.data;
    if(self.actions.hasOwnProperty(data.ID)){
      self.actions[data.ID] = false;
      if(self.dbg) console.log('onActionDisconnect', data)
      delete self.actions[data];
      window.Socket.data.actionsCount--;
    }
    else
    {
      if(self.dbg) console.log('onActionDisconnect (there were none)', data);
    }
  }

  /*
    ADMINS
  */

  onAdminConnect(data){
    var self = window.Socket.data;
    for(var i = 0; i < self.dashboardSessions.length; i++)
      if(self.dashboardSessions[i] == data.Username)
        return;

    self.dashboardSessionsCount++;
    self.dashboardSessions.push(data.Username);
    if(self.dbg)
      console.log('onAdminConnect', data);
  }

  onAdminDisconnect(data){
    var self = window.Socket.data;
    for(var i = 0; i < self.dashboardSessions.length; i++)
      if(self.dashboardSessions[i] == data.Username){
        self.dashboardSessionsCount--;
        self.dashboardSessions.splice(i);
      }
    if(self.dbg)
      console.log('onAdminDisconnect', data);
  }


  /*
    MAPPING
  */

  mapActions(input, data){
    if(typeof data === 'object')
      for(var i = 0; i < data.length; i++)
        if(!input.hasOwnProperty(data[i].aid))
        {
          input[data[i].aid] = data[i];
          socket.data.actionsCount++;
        }
    return input;
  }

  mapValues(input, data) {
    //input = Object;
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