using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Direct
{

  public enum CCSubmitConnectionStringType
  {
    Live,
    Livesports,
    DebugApi,
    DebugDesktop,
    LocalDV
  }

  public static class CCSubmitConnectionString
  {

    public static CCSubmitConnectionStringType Type = CCSubmitConnectionStringType.Live;

    private static string[] ConnectionStrings = new string[]
    {
      // Live
      "Server=ccmonkeys.cerqlxjx1slg.eu-central-1.rds.amazonaws.com; database=ccmonkeys; UID=admin; password=adminpasssifra12345; Allow User Variables=True;",

      // Livesports
      "Server=46.166.160.58; database=livesports; UID=livesports; password=a48i72V\"B?8>79Z",

      // DebugApi
      "Server=ccmonkeys.cerqlxjx1slg.eu-central-1.rds.amazonaws.com; database=ccmonkeys; UID=admin; password=adminpasssifra12345; Allow User Variables=True;",
      
      // DebugDesktop
      "Server=ccmonkeys.cerqlxjx1slg.eu-central-1.rds.amazonaws.com; database=ccmonkeys; UID=admin; password=adminpasssifra12345; Allow User Variables=True;",
      // LocalDV
      "Server=127.0.0.1; UID=root; password=; database=test; Allow User Variables=True;"

    };

    internal static string GetConnectionString()
    {
      #if DEBUG
        return ConnectionStrings[(int)CCSubmitConnectionStringType.Live];
      #else 
        return ConnectionStrings[(int)Type];
      #endif
    }

  }
}
