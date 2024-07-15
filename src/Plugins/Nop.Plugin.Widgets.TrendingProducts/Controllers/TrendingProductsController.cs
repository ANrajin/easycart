using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.TrendingProducts.Infrastructure.Securities;
using Nop.Plugin.Widgets.TrendingProducts.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
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
    ILocalizationService localizationService,
    IPermissionService permissionService) : BasePluginController
{
    private readonly IStoreContext _storeContext = storeContext;
    private readonly ISettingService _settingService = settingService;
    private readonly INotificationService _notificationService = notificationService;
    private readonly ILocalizationService _localizationService = localizationService;
    private readonly IPermissionService _permissionService = permissionService;

    public async Task<IActionResult> ConfigureAsync()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
            return AccessDeniedView();

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
    public async Task<IActionResult> ConfigureAsync(ConfigurationModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
            return AccessDeniedView();

        if (ModelState.IsValid)
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

    public async Task<IActionResult> ListAsync()
    {
        if(!await _permissionService.AuthorizeAsync(TrendingProductsPermissionProvider.ViewTrendingProducts))
            return AccessDeniedView();

        return View();
    }
}
