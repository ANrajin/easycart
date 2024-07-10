using Nop.Web.Framework.Models;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Widgets.TrendingProducts.Models;
public record TrendingProductsPublicInfoModel : BaseNopModel
{
    public SliderSettingsModel SliderSettings { get; set; }

    public IList<ProductOverviewModel> Products { get; set; }
}

public record SliderSettingsModel
{
    public int SlidesToShow { get; set; } = 4;
    public int SlidesToScroll { get; set; } = 1;
    public bool AutoPlay { get; set; }
    public int AutoPlaySpeed { get; set; } = 2000;
}
