using Nop.Core;

namespace Nop.Plugin.Widgets.HelloWorld.Domain;
public class Banner:BaseEntity
{
    public string AltText { get; set; }
    public string LinkText { get; set; }
    public string ImageUrl { get; set; }
}
