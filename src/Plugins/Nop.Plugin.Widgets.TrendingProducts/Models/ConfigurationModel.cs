using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.TrendingProducts.Models;
public record ConfigurationModel : BaseNopModel
{
    [UIHint("DateTime")]
    public DateTime FromDate { get; set; }

    [UIHint("DateTime")]
    public DateTime ToDate { get; set; }

    [UIHint("Number"), MinLength(1), MaxLength(20)]
    public int Count { get; set; }

    public int ActiveStoreScopeConfiguration { get; set; }
}
