using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.HelloWorld.Models;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.HelloWorld.Controllers;

[Area(AreaNames.ADMIN)]
[AuthorizeAdmin]
[AutoValidateAntiforgeryToken]
public class HelloWorldAdminController:BasePluginController
{
    public IActionResult Configure()
    {
        var model = new ConfigurationModel();

        return View("~/Plugins/Widgets.HelloWorld/Views/Configure.cshtml",model);
    }
}
