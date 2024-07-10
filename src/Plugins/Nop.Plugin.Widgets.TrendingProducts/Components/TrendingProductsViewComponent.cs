using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.TrendingProducts.Factories;
using Nop.Plugin.Widgets.TrendingProducts.Services;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.TrendingProducts.Components;
public class TrendingProductsViewComponent(
    ITrendingProductFactory trendingProductFactory,
    ITrendingProductService trendingProductService,
    ISettingService settingService) : NopViewComponent
{
    private readonly ITrendingProductFactory _trendingProductFactory = trendingProductFactory;
    private readonly ITrendingProductService _trendingProductService = trendingProductService;
    private readonly ISettingService _settingService = settingService;

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var setting = await _settingService.LoadSettingAsync<TrendingProductsSetting>();

        var products = await _trendingProductService.GetTrendingProducts(setting, 1);

        var model = await _trendingProductFactory.PreparePublicInfoModelAsync(products, setting);

        return View(model);
    }
}
