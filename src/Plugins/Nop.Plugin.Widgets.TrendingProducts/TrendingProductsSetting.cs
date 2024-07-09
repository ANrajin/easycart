using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.TrendingProducts;
public sealed class TrendingProductsSetting:ISettings
{
    public DateTime FromDate { get; set; } = DateTime.Now.AddDays(-30);

    public DateTime ToDate { get; set; } = DateTime.Now;

    public int Count { get; set; } = 10;
}
