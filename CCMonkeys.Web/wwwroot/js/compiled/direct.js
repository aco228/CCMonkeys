class Direct{


  constructor(){
    if(typeof window.Socket === 'undefined')
      throw Exception('socket is not defined breee');

    this.callbacks = [];
  }

  query(){
    return new Query();
  }

  sql(val, callback){
    if(typeof callback !== 'function') {
      console.error(`DIRECT:: FATAL: Load must have valid callback function`);
      return;
    }
    let query = new Query();
    query.sql(val);
    this.load(query, callback);
  }

  onMessage(data){
    try
    {
      console.log(data);
      if(data.Success == false && data.Ticket == '') {
        // some kind of global error
        console.error('DIRECT:: FATAL ' + data.Message);
        return;
      }

      var entry = null;
      for(var i = 0; i < this.callbacks.length; i++)
        if(this.callbacks[i].query.data.ticket == data.Ticket)
        {
          entry = this.callbacks[i];
          this.callbacks.splice(i, 1);
          break;
        }

      console.log(entry);
      if(entry == null){
        // we have some sort of error because we do not have any callback asign to this 
        console.error(`DIRECT:: FATAL: Strange error, ticket #${data.Ticket} does not have any callbacks`);
        return;
      }

      var ms = ((new Date()).getTime() - entry.query.created.getTime());
      var count = data.Data != null ? data.Data.length : 0;
      console.warn(`DIRECT:: #${data.Ticket} got response in ${ms}ms (${count} responses)`);
      if(data.Success == false){
        console.error(`DIRECT:: ERROR: with message ${data.Message}`);
        entry.callback(null);
      }
      else
      {
        entry.callback(data.Data);
      }

    }
    catch(ex)
    {
      console.error(`DIRECT:: FATAL: STRUCTURAL FATAL`, ex);
    }
  }

  load(query, callback){
    if(typeof callback !== 'function') {
      console.error(`DIRECT:: FATAL: Load must have valid callback function`);
      return;
    }

    query.data.ticket = this.makeid();
    window.Socket.send('direct', query.data);
    query.created = new Date();
    this.callbacks.push({query: query, callback: callback});
  }


  makeid() {
    var length = 10;
    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;
    for ( var i = 0; i < length; i++ ) 
       result += characters.charAt(Math.floor(Math.random() * charactersLength));
    return result;
}

}

class Query{

  constructor(){
    this.data = {};
    this.data.query = '';
    this.data.from = '';
    this.data.select = '';
    this.data.where = '';
    this.data.orderBy = '';
    this.data.limit = '';
  }

  sql(val){
    this.data.query = val;
    return this;
  }

  f (val) { return this.from(val); }
  from(val){
    this.data.from = val;
    return this;
  }

  s(val){ return this.select(val); }
  select(val){
    this.data.select = val;
    return this;
  }

  w(val){ return this.where(val); }
  where(val){
    this.data.where = val;
    return this;
  }

  o(val){ return this.orderBy(val); }
  orderBy(val){
    this.data.orderBy = val;
    return this;
  }

  l(val){ return this.limit(val); }
  limit(val){
    this.data.limit = val;
    return this;
  }

  load(callback){
    window.Direct.load(this, callback);
  }


}


window.Direct = new Direct();