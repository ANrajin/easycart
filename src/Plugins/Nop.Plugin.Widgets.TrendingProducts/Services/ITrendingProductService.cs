using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Widgets.TrendingProducts.Services;
public interface ITrendingProductService
{
    Task<IList<Product>> GetTrendingProducts(TrendingProductsSetting setting, int storeId);
}
