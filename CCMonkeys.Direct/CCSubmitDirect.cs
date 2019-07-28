using Direct;
using Direct.Types.Mysql;
using System;

namespace CCMonkeys.Direct
{
  public class CCSubmitDirect : DirectDatabaseMysql
  {
    private static object LockObj = new object();
    private static CCSubmitDirect _instance = null;

    public static CCSubmitDirect Instance
    {
      get
      {
        //if (_instance != null)
        //  return _instance;
        //_instance = new CCSubmitDirect();
        //return _instance;
        return new CCSubmitDirect();
      }
    }

    public CCSubmitDirect()
      : base("ccmonkeys", CCSubmitConnectionString.GetConnectionString())
    { }


    public override void OnException(DirectDatabaseExceptionType type, string query, Exception e)
    {
      int a = 0;
    }
  }
}
