using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.HelloWorld.Domain;
using Nop.Plugin.Widgets.HelloWorld.Factories;
using Nop.Plugin.Widgets.HelloWorld.Models;
using Nop.Plugin.Widgets.HelloWorld.Services;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.HelloWorld.Controllers;

[Area(AreaNames.ADMIN)]
[AuthorizeAdmin]
[AutoValidateAntiforgeryToken]
public class HelloWorldHomeController(IBannerService bannerService,
    INotificationService notificationService,
    ILocalizationService localizationService,
    IBannerModelFactory bannerModelFactory):BasePluginController
{
    private readonly IBannerService _bannerService = bannerService;
    private readonly INotificationService _notificationService = notificationService;
    private readonly ILocalizationService _localizationService = localizationService;
    private readonly IBannerModelFactory _bannerModelFactory = bannerModelFactory;

    public async Task<IActionResult> HelloWorldIndex()
    {
        BannerModel model = new();

        var banner = await _bannerService.GetBannerAsync();

        if(banner is not null)
        {
            model = await _bannerModelFactory.PrepareBannerModelAsync(null, banner);
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdateAsync(BannerModel model)
    {
        if(ModelState.IsValid)
        {
            var banner = model.ToEntity<Banner>();

            await _bannerService.UpdateBannerAsync(banner);

            _notificationService.SuccessNotification(
                await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));


            return RedirectToAction("HelloWorldIndex", "HelloWorldHome");
        }

        model = await _bannerModelFactory.PrepareBannerModelAsync(model, null);

        return View("HelloWorldIndex", model);
    }
}
