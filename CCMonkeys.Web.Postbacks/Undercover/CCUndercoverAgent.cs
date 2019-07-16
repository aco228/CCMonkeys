using CCMonkeys.Direct;
using Direct.ccmonkeys.Models;
using Direct.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CCMonkeys.Web.Postbacks.Undercover
{

  public class CCUndercoverAgent
  {
    public static UndercoverResult Init(ActionDM action, PostbackDM postback)
    {
      postback.Log("CC_NEW:: Starting for : " + action.trackingid);
      UndercoverResult result = new UndercoverResult();
      CCSubmitDirect db = CCSubmitDirect.Instance;

      string affID = action.affid;
      string pubID = action.pubid;

      if (string.IsNullOrEmpty(affID))
      {
        postback.Log("CC_NEW:: There is no AffID for clickID: " + action.trackingid);
        return result;
      }

      DirectContainer _directContainer;
      if (!string.IsNullOrEmpty(pubID))
        _directContainer = db.LoadContainer("SELECT * FROM [].cc_undercover WHERE (affid={0} AND pubid={1}) OR (affid={0} AND pubid IS NULL) ORDER BY pubid DESC LIMIT 1;", int.Parse(affID), pubID);
      else
        _directContainer = db.LoadContainer("SELECT * FROM [].cc_undercover WHERE (affid={0} AND pubid IS NULL) ORDER BY pubid DESC LIMIT 1;", affID);


      if (_directContainer.HasValue && _directContainer.GetDouble("tcost").HasValue)
        try
        {
          return GetByVariableTCost(_directContainer, action, postback);
        }
        catch (Exception e)
        {
          postback.Log("[FATAL WITH tcost] " + e.ToString());
        }

      postback.Log("CCUndercover will go to old way, for clickID = " + action.trackingid + ", affID=" + affID);
      return result;
    }

    private static UndercoverResult GetByVariableTCost(DirectContainer container, ActionDM action, PostbackDM postback)
    {
      UndercoverResult result = null;
      var banana = new BananaclicksUndercoverManager();
      double tcost = container.GetDouble("tcost").Value;

      //
      // Affiliate price from bananaclicks

      double? affPrice = banana.GetAffiliateProfit(action.affid);
      if (!affPrice.HasValue)
      {
        postback.Log("[FATAL]:: THERE IS NO affPrice for affiliate=" + action.affid);
        return null;
      }
      affPrice = DollarConversion.Convert(affPrice.Value);

      //
      // Current transactions from database

      int? currentTransactions = banana.GetAffiliateCurrentTransactions(action.affid);
      if (!currentTransactions.HasValue)
      {
        postback.Log("[FATAL]:: THERE IS NO currentTransactions for affiliate=" + action.affid);
        return null;
      }
      else
        currentTransactions += 1;


      //
      // Current price [bananaclicks price] / [conversions from database]

      double current_price = 0.0;
      if (affPrice.Value == 0.0 || affPrice.Value > 0 && currentTransactions.Value == 1)
        current_price = 0.0;
      else
        current_price = affPrice.Value / (currentTransactions.Value * 1.0);

      string logString = string.Format("affID={0}, tcost={1}, affiliate_profit={2}, current_price={3}, current_transactions={4} ", action.affid, tcost, Math.Round(affPrice.Value, 2), Math.Round(current_price, 2), currentTransactions.Value);

      //
      // Logic for undercover

      if (current_price == 0 || current_price <= tcost)
      {
        logString += " (REPORT)";
        result = UndercoverResult.SendToBananaclicks();
      }
      else
      {
        logString += " THIS WILL NOT BE REPORTED";
        result = new UndercoverResult() { DontSendConversionToBananaclicks = true };
      }

      postback.Log(logString);
      return result;
    }

    private static string GetParamByName(string input, string parameterName)
    {
      Match match = new Regex(string.Format(@"(\?{0}=([A-Za-z0-9]+))|(\&{0}=([A-Za-z0-9]+))", parameterName)).Match(input);
      string result = string.Empty;
      if (!string.IsNullOrEmpty(match.Groups[2].Value.ToString()))
        result = match.Groups[2].Value.ToString();
      else if (string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(match.Groups[4].Value.ToString()))
        result = match.Groups[4].Value.ToString();
      return result;
    }

  }
}
