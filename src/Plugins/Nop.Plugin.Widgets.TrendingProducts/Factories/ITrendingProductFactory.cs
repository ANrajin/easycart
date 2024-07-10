using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.TrendingProducts.Models;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Widgets.TrendingProducts.Factories;
public interface ITrendingProductFactory
{
    Task<TrendingProductsPublicInfoModel> PreparePublicInfoModelAsync(
        IEnumerable<Product> products,
        TrendingProductsSetting settings);
}
