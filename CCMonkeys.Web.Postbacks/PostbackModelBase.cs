using CCMonkeys.Direct;
using Direct.ccmonkeys.Models;
using Direct.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Postbacks
{
  public abstract class PostbackModelBase
  {
    public string TrackingID { get; protected set; } = string.Empty;
    public bool IsValid { get; protected set; } = true;

    public ActionModelEvent Type { get; protected set; } = ActionModelEvent.Default;

    public void Prepare(HttpRequest request)
    {
      this.Init(request);

      this.Type = this.GetType();
      this.TrackingID = this.GetTrackingGuid();

    }

    protected abstract void Init(HttpRequest Request);
    protected abstract ActionModelEvent GetType();
    protected abstract string GetTrackingGuid();

  }
}
