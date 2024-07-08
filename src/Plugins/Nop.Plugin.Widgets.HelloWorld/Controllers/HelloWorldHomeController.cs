using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.HelloWorld.Domain;
using Nop.Plugin.Widgets.HelloWorld.Models;
using Nop.Plugin.Widgets.HelloWorld.Services;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.HelloWorld.Controllers;

[Area(AreaNames.ADMIN)]
[AuthorizeAdmin]
[AutoValidateAntiforgeryToken]
public class HelloWorldHomeController(IBannerService bannerService,
    INotificationService notificationService,
    ILocalizationService localizationService):BasePluginController
{
    private readonly IBannerService _bannerService = bannerService;
    private readonly INotificationService _notificationService = notificationService;
    private readonly ILocalizationService _localizationService = localizationService;

    public async Task<IActionResult> HelloWorldIndex()
    {
        var banner = await _bannerService.GetBannerAsync();
        var model = new BannerModel()
        {
            AltText = banner.AltText,
            LinkText = banner.LinkText,
            ImageUrl = Convert.ToInt32(banner.ImageUrl),
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdateAsync(BannerModel model)
    {
        var banner = new Banner()
        {
            AltText = model.AltText,
            LinkText = model.LinkText,
            ImageUrl = model.ImageUrl.ToString(),
        };
        await _bannerService.InsertBannerAsync(banner);
        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));
        return RedirectToAction("HelloWorldIndex", "HelloWorldHome");
    }
}
