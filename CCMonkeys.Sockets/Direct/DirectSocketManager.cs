using CCMonkeys.Direct;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Sockets.Direct
{
  public class DirectSocketManager
  {

    public static void OnRequestLoad(ServerSocketBase server, string key, string socket_uid, string rawDataInput)
    {
      DirectSocketResponse response = new DirectSocketResponse();
      try
      {
        DirectSocketInputModel input = JsonConvert.DeserializeObject<DirectSocketInputModel>(rawDataInput);
        response.Ticket = input.ticket;
        string query = "";

        if (!string.IsNullOrEmpty(input.query))
        {
          if (!input.query.Trim().ToLower().StartsWith("select"))
            throw new Exception("ONLY SELECT IS IN USE!!!");
          query = input.query;
        }
        else
          query = string.Format($"SELECT {input.SelectQuery}{input.FromQuery}{input.WhereQuery}{input.OrderByQuery}{input.LimitQuery}");

        CCSubmitDirect db = CCSubmitDirect.Instance;
        response.Data = db.Load(query).RawData;
      }
      catch(Exception e)
      {
        response.Success = false;
        response.Message = e.ToString();
      }
      finally
      {
        server.SendAsync(socket_uid, JsonConvert.SerializeObject(response));
      }
    }


  }
}
