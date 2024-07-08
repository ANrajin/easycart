using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.HelloWorld.Models;
using Nop.Plugin.Widgets.HelloWorld.Services;
using Nop.Services.Media;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.HelloWorld.Components;
public class HelloWorldViewComponent(
    IPictureService pictureService,
    IBannerService bannerService) : NopViewComponent
{
    private readonly IPictureService _pictureService = pictureService;
    private readonly IBannerService _bannerService = bannerService;

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var banner = await _bannerService.GetBannerAsync();
        var model = new PublicBannerModel
        {
            AltText = banner.AltText,
            LinkText = banner.LinkText,
            ImageUrl = await _pictureService.GetPictureUrlAsync(
                Convert.ToInt32(banner.ImageUrl), 
                showDefaultPicture: false) ?? ""
        };

        return View("~/Plugins/Widgets.HelloWorld/Views/HelloWorld.cshtml", model);
    }
}
