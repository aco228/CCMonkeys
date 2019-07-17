﻿using Direct.Core.Mysql;
using System;
using static Direct.Core.DirectDatabaseBase;

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