using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.HelloWorld.Models;
public record PublicBannerModel : BaseNopModel
{
    public string AltText { get; set; }
    public string LinkText { get; set; }
    public string ImageUrl { get; set; }
}
