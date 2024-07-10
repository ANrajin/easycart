using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.TrendingProducts;
public sealed class TrendingProductsSetting:ISettings
{
    public DateTime FromDate { get; set; } = DateTime.Now.AddDays(-30);

    public DateTime ToDate { get; set; } = DateTime.Now;

    public int Count { get; set; } = 10;

    public int SlidesToShow { get; set; } = 4;

    public int SlidesToScroll { get; set; } = 1;

    public bool AutoPlay { get; set; }

    public int AutoPlaySpeed { get; set; } = 2000;
}
