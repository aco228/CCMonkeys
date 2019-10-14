using CCMonkeys.Direct;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Code.CacheManagers.Core
{
  public abstract class CacheManagerBase
  {
    protected CCSubmitDirect Database { get; private set; }
    protected virtual bool IsInitCompleted { get; set; } = false;
    protected DateTime? LastInit { get; set; } = null;

    public void Construct(CCSubmitDirect db)
    {
      this.Database = db;
      this.CallInit();
    }

    protected void CallInit()
    {
      this.Init();
      this.IsInitCompleted = true;
      LastInit = DateTime.Now;
    }

    public virtual void Reload()
    {
      this.ClearData();
      this.Init();
      this.LastInit = DateTime.Now;
    }

    protected abstract void ClearData();
    protected abstract void Init();


  }
}
