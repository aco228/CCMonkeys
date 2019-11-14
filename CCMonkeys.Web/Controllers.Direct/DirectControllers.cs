using Direct.ccmonkeys.Models;
using Direct.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Controllers.Direct
{

  // Prelander

  [Route("direct/api/tm_lander")]
  public class DirectLanderController : DirectControllerBase<LanderDM> { }

  [Route("direct/api/tm_lander_param_info")]
  public class DirectLanderParamInfoController : DirectControllerBase<LanderParamInfoDM> { }

  [Route("direct/api/tm_landertype")]
  public class DirectLanderTypeController : DirectControllerBase<LanderTypeDM> { }

  // Prelander

  [Route("direct/api/tm_prelander")]
  public class DirectPrelanderController : DirectControllerBase<PrelanderDM> { }

  [Route("direct/api/tm_prelandertype")]
  public class DirectPrelanderTypeController : DirectControllerBase<PrelanderTypeDM> { }

  [Route("direct/api/tm_prelander_domain")]
  public class DirectPrelanderDomainController : DirectControllerBase<PrelanderDomainDM> { }

  // Admin

  [Route("direct/api/tm_admin")]
  public class DirectAdmin : DirectControllerBase<AdminDM> { }

  [Route("direct/api/tm_provider")]
  public class DirectProvider : DirectControllerBase<ProviderDM> { }

}
