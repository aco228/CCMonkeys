using CCMonkeys.Direct;
using System;

namespace CCMonkeys.Consoles.GenerateModels
{
  class Program
  {
    static void Main(string[] args)
    {
      var db = new CCSubmitDirect();
      db.ModelsCreator.GenerateFile("tm_user", "User", @"D:\github\CCMonkeys\_rest\output");
      //db.ModelsCreator.GenerateFile("cc_postback_log", "PostbackLog", @"D:\github\CCMonkeys\_rest\output");
      //db.ModelsCreator.GenerateFile("cc_undercover", "Undercover", @"D:\github\CCMonkeys\_rest\output");
      //db.ModelsCreator.GenerateFile("tm_action", "Action", @"D:\github\CCMonkeys\_rest\output");
      //db.ModelsCreator.GenerateFile("tm_action_account", "ActionAccount", @"D:\github\CCMonkeys\_rest\output");
      //db.ModelsCreator.GenerateFile("tm_country", "Country", @"D:\github\CCMonkeys\_rest\output");
      //db.ModelsCreator.GenerateFile("tm_lander", "Lander", @"D:\github\CCMonkeys\_rest\output");
      //db.ModelsCreator.GenerateFile("tm_lead", "Lead", @"D:\github\CCMonkeys\_rest\output");
      //db.ModelsCreator.GenerateFile("tm_lead_history", "LeadHistory", @"D:\github\CCMonkeys\_rest\output");
      //db.ModelsCreator.GenerateFile("tm_prelander", "Prelander", @"D:\github\CCMonkeys\_rest\output");
      //db.ModelsCreator.GenerateFile("tm_provider", "Provider", @"D:\github\CCMonkeys\_rest\output");
      //db.ModelsCreator.GenerateFile("tm_session", "Session", @"D:\github\CCMonkeys\_rest\output");
      //db.ModelsCreator.GenerateFile("tm_session_data", "SessionData", @"D:\github\CCMonkeys\_rest\output");
      //db.ModelsCreator.GenerateFile("tm_session_request", "SessionRequest", @"D:\github\CCMonkeys\_rest\output");
      //db.ModelsCreator.GenerateFile("tm_user", "User", @"D:\github\CCMonkeys\_rest\output");
    }
  }
}
