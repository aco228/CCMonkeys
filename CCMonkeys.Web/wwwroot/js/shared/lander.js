CC.type = 'lp';
CC.lander = {

  createUserModel: { email: '' },
  createUser: function(callback){ CC.api.send('user-create', this.createUserModel, callback); },

  subscribeUserModel: {
    firstName: '',
    lastName: '',
    address: '',
    city: '',
    postcode: '',
    msisdn: '',
    country: ''
  },
  subscribeUser: function(callback){ CC.api.send('user-subscribe', this.subscribeUserModel, callback); },


  userRedirectedModel: { url: '' },
  userRedirected: function(callback){ CC.api.send('user-redirected', this.userRedirectedModel, callback); }
};