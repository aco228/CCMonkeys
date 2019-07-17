using CCMonkeys.Direct;
using System;

namespace CCMonkeys.Consoles.Migration
{
  class Program
  {
    static void Main(string[] args)
    {
      LivesportsDatabase livesportsDb = new LivesportsDatabase();
      CCSubmitDirect db = new CCSubmitDirect();

      for(; ; )
      {
        var dc = livesportsDb.LoadContainerAsync("SELECT ");
      }

      Console.WriteLine("Hello World!");
    }
  }
}
