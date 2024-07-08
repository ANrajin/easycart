using Nop.Plugin.Widgets.HelloWorld.Domain;
using Nop.Plugin.Widgets.HelloWorld.Models;

namespace Nop.Plugin.Widgets.HelloWorld.Factories;
public class BannerModelFactory : IBannerModelFactory
{
    public async Task<BannerModel> PrepareBannerModelAsync(BannerModel model, Banner banner)
    {
        if(model == null)
        {
            model = new BannerModel
            {
                AltText = banner.AltText,
                LinkText = banner.LinkText,
                ImageUrl = banner.ImageUrl,
            };
        }

        return model;
    }
}
