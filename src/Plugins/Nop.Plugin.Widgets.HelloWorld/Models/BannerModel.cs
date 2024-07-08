using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.HelloWorld.Models;
public record BannerModel : BaseNopEntityModel
{
    #region Properties 
    public string AltText { get; set; }
    
    public string LinkText { get; set; }

    [UIHint("Picture")]
    public int ImageUrl { get; set; }
    #endregion
}
