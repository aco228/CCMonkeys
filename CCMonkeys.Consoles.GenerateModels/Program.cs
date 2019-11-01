using CCMonkeys.Direct;
using Direct.Types.Mysql;
using System;

namespace CCMonkeys.Consoles.GenerateModels
{
  class Program
  {
    static void Main(string[] args)
    {
      var db = new CCSubmitDirect();
      var creator = new MysqlModelsGenerator(db);

      creator.GenerateFile("tm_provider", "Provider", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_action_account", "ActionAccount", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_admin", "Admin", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_admin_session", "AdminSession", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_country", "Country", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_country_used", "CountryUsed", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_email_blacklist", "EmailBlacklist", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_lander", "Lander", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_landertype", "LanderType", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_lead", "Lead", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_lead_history", "LeadHistory", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_prelander", "Prelander", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_prelandertype", "PrelanderType", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_provider", "Provider", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_session", "Session", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_session_data", "SessionData", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_session_request", "SessionRequest", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_session_type", "SessionType", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_test", "Test", @"D:\github\CCMonkeys\_rest\output");
      //creator.GenerateFile("tm_user", "User", @"D:\github\CCMonkeys\_rest\output");
    }
  }
}
