using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.TrendingProducts.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.TrendingProducts.Controllers;

[Area(AreaNames.ADMIN)]
[AuthorizeAdmin]
[AutoValidateAntiforgeryToken]
public class TrendingProductsController(
    IStoreContext storeContext,
    ISettingService settingService,
    INotificationService notificationService,
    ILocalizationService localizationService) : BasePluginController
{
    private readonly IStoreContext _storeContext = storeContext;
    private readonly ISettingService _settingService = settingService;
    private readonly INotificationService _notificationService = notificationService;
    private readonly ILocalizationService _localizationService = localizationService;

    public async Task<IActionResult> ConfigureAsync()
    {
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var trendingProductsSettings = await _settingService.LoadSettingAsync<TrendingProductsSetting>();

        var model = new ConfigurationModel
        {
            FromDate = trendingProductsSettings.FromDate,
            ToDate = trendingProductsSettings.ToDate,
            Count = trendingProductsSettings.Count,
            SlidesToShow = trendingProductsSettings.SlidesToShow,
            SlidesToScroll = trendingProductsSettings.SlidesToScroll,
            AutoPlay = trendingProductsSettings.AutoPlay,
            AutoPlaySpeed = trendingProductsSettings.AutoPlaySpeed,
            ActiveStoreScopeConfiguration = storeScope
        };

        return View("~/Plugins/Widgets.TrendingProducts/Views/Configure.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        if(ModelState.IsValid)
        {
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var trendingProductsSettings = await _settingService.LoadSettingAsync<TrendingProductsSetting>();

            trendingProductsSettings.FromDate = model.FromDate;
            trendingProductsSettings.ToDate = model.ToDate;
            trendingProductsSettings.Count = model.Count;
            trendingProductsSettings.AutoPlay = model.AutoPlay;
            trendingProductsSettings.SlidesToScroll = model.SlidesToScroll;
            trendingProductsSettings.SlidesToShow = model.SlidesToShow;
            trendingProductsSettings.AutoPlaySpeed = model.AutoPlaySpeed;

            await _settingService.SaveSettingAsync(trendingProductsSettings, storeScope);

            _notificationService.SuccessNotification(
                await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return RedirectToAction("Configure", "TrendingProducts");
        }

        return View(model);
    }
}
