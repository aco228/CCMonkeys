using Direct;
using Direct.Types.Mysql;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Consoles.Migration
{
  class LivesportsDatabase : DirectDatabaseMysql
  {
    private static object LockObj = new object();
    private static LivesportsDatabase _instance = null;

    public static LivesportsDatabase Instance
    {
      get
      {
        return new LivesportsDatabase();
      }
    }

    public LivesportsDatabase()
      : base("livesports", "Server=46.166.160.58; database=livesports; UID=livesports; password=a48i72V\"B?8>79Z")
    { }


    public override void OnException(DirectDatabaseExceptionType type, string query, Exception e)
    {
      int a = 0;
    }
  }
}
