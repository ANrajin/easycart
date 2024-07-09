using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.TrendingProducts.Models;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.TrendingProducts.Controllers;

[Area(AreaNames.ADMIN)]
[AuthorizeAdmin]
[AutoValidateAntiforgeryToken]
public class TrendingProductsController : BasePluginController
{
    public IActionResult Configure()
    {
        var model = new ConfigurationModel();
        return View("~/Plugins/Widgets.TrendingProducts/Views/Configure.cshtml", model);
    }
}
