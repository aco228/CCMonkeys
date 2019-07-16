using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Postbacks.Undercover
{
  public class UndercoverResult
  {
    public bool DontSendConversionToBananaclicks = false;

    public static UndercoverResult SendToBananaclicks() => new UndercoverResult() { DontSendConversionToBananaclicks = false };
    public static UndercoverResult DontSendToBananaclicks() => new UndercoverResult() { DontSendConversionToBananaclicks = true };
  }
}
