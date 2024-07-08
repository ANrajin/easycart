using Nop.Plugin.Widgets.HelloWorld.Domain;
using Nop.Plugin.Widgets.HelloWorld.Models;
using Nop.Plugin.Widgets.HelloWorld.Services;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;

namespace Nop.Plugin.Widgets.HelloWorld.Factories;
public class BannerModelFactory(IBannerService bannerService) : IBannerModelFactory
{
    private readonly IBannerService _bannerService = bannerService;

    public async Task<BannerModel> PrepareBannerModelAsync(BannerModel model, Banner banner)
    {
        if(model == null)
        {
            model = banner.ToModel<BannerModel>();
        }

        if(model.ImageUrl == 0)
        {
            banner = await _bannerService.GetBannerAsync();
            model.ImageUrl = banner.ImageUrl;
        }

        return model;
    }
}
