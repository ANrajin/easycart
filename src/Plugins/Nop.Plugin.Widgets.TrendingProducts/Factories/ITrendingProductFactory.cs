using Nop.Core.Domain.Catalog;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Widgets.TrendingProducts.Factories;
public interface ITrendingProductFactory
{
    Task<IEnumerable<ProductOverviewModel>> PrepareProductOverviewModelAsync(
        IEnumerable<Product> product);
}
