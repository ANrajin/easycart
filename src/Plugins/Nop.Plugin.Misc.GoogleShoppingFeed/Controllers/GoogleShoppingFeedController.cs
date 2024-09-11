using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Misc.GoogleShoppingFeed.Controllers;
public class GoogleShoppingFeedController : BaseController
{
    public async Task<IActionResult> Configure()
    {
        return View();
    }
}
