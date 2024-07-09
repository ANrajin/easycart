using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.TrendingProducts.Models;
using Nop.Services.Configuration;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.TrendingProducts.Controllers;

[Area(AreaNames.ADMIN)]
[AuthorizeAdmin]
[AutoValidateAntiforgeryToken]
public class TrendingProductsController(
    IStoreContext storeContext,
    ISettingService settingService) : BasePluginController
{
    private readonly IStoreContext _storeContext = storeContext;
    private readonly ISettingService _settingService = settingService;

    public async Task<IActionResult> ConfigureAsync()
    {
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var trendingProductsSettings = await _settingService.LoadSettingAsync<TrendingProductsSetting>();

        var model = new ConfigurationModel
        {
            FromDate = trendingProductsSettings.FromDate,
            ToDate = trendingProductsSettings.ToDate,
            Count = trendingProductsSettings.Count,
            ActiveStoreScopeConfiguration = storeScope
        };

        return View("~/Plugins/Widgets.TrendingProducts/Views/Configure.cshtml", model);
    }
}
