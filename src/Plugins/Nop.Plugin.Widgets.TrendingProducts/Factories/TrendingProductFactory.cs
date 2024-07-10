using Nop.Core.Domain.Catalog;
using Nop.Web.Factories;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Widgets.TrendingProducts.Factories;
public sealed class TrendingProductFactory(IProductModelFactory productModelFactory)
    : ITrendingProductFactory
{
    private readonly IProductModelFactory _productModelFactory = productModelFactory;

    public async Task<IEnumerable<ProductOverviewModel>> PrepareProductOverviewModelAsync(IEnumerable<Product> products)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(products));

        var models = await _productModelFactory.PrepareProductOverviewModelsAsync(products, true);

        return models;
    }
}
