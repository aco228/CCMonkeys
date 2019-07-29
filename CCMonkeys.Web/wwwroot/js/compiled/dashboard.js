class Socket{

  get host() { return '[HOST]'; }
  get sguid() { return '[SGUID]'; }
  get events() { return{[EVENTS]} }

  constructor(){
    this.callbacks = [];
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
      this.send('register', {sguid:this.sguid});
      this.register = true;
    }
  }

  onClose(e){
    this.console("onClose", "connection closed", e);
  }

  onMessage(e){
    var response = JSON.parse(e.data);
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

}


if (typeof window === 'undefined') exports.Dashboard = new Socket();
else  window.Socket = new Socket();