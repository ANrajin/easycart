using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.TrendingProducts.Models;
using Nop.Web.Factories;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Widgets.TrendingProducts.Factories;
public sealed class TrendingProductFactory(IProductModelFactory productModelFactory)
    : ITrendingProductFactory
{
    private readonly IProductModelFactory _productModelFactory = productModelFactory;

    public async Task<TrendingProductsPublicInfoModel> PreparePublicInfoModelAsync(
        IEnumerable<Product> products, 
        TrendingProductsSetting settings)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(products));

        var productOverviewModels = await _productModelFactory.PrepareProductOverviewModelsAsync(products, true);

        var sliderSettings = new SliderSettingsModel
        {
            AutoPlay = settings.AutoPlay,
            AutoPlaySpeed = settings.AutoPlaySpeed,
            SlidesToShow = settings.SlidesToShow,
            SlidesToScroll = settings.SlidesToScroll,
        };

        return new TrendingProductsPublicInfoModel
        {
            Products = productOverviewModels.ToList(),
            SliderSettings = sliderSettings,
        };
    }
}
