using Nop.Plugin.Widgets.TrendingProducts.Dtos;

namespace Nop.Plugin.Widgets.TrendingProducts.Services;
public interface ITrendingProductService
{
    Task<IList<TrendingProductDto>> GetTrendingProducts();
}
