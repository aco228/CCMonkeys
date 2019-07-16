ActionDM action = null;

      using (var db = new CCSubmitDirect())
      {
        CountryDM country = db.Query<CountryDM>()
          .Select("").Where("code={0}", "me")
          .LoadLater();
        
        UserDM user = new UserDM(db) { guid = Guid.NewGuid().ToString(), };
        user.Link(country).InsertLater();
        
        SessionDM session = new SessionDM(db) { guid = Guid.NewGuid().ToString(), sessiontype = 1 };
        session.Link(user).InsertLater();

        action = new ActionDM(db) { };
        action.Link(user, session, country).InsertLater();
      }

      Console.WriteLine(string.Format("action.actionid: {0}", (action.ID.HasValue ? action.ID.ToString() : "NULL")));
      Console.WriteLine(string.Format("action.countryid: {0}", (action.countryid.HasValue ? action.countryid.ToString() : "NULL")));
      Console.WriteLine(string.Format("action.userid: {0}", (action.userid.ToString())));
      Console.WriteLine(string.Format("action.sessionid: {0}", (action.sessionid.ToString())));

      Console.WriteLine();
      Console.WriteLine("Ovjde cemo 5 sekundi nesto da radimo drugo");
      System.Threading.Thread.Sleep(5000);
      Console.WriteLine();

      Console.WriteLine(string.Format("action.actionid: {0}", (action.ID.HasValue ? action.ID.ToString() : "NULL")));
      Console.WriteLine(string.Format("action.countryid: {0}", (action.countryid.HasValue ? action.countryid.ToString() : "NULL")));
      Console.WriteLine(string.Format("action.userid: {0}", (action.userid.ToString())));
      Console.WriteLine(string.Format("action.sessionid: {0}", (action.sessionid.ToString())));
