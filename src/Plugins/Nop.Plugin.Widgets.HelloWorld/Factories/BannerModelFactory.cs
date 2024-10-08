﻿using Nop.Plugin.Widgets.HelloWorld.Domain;
using Nop.Plugin.Widgets.HelloWorld.Models;
using Nop.Plugin.Widgets.HelloWorld.Services;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Widgets.HelloWorld.Factories;
public class BannerModelFactory(IBannerService bannerService) : IBannerModelFactory
{
    private readonly IBannerService _bannerService = bannerService;

    public async Task<BannerListModel> PrepareBannerListModelAsync()
    {
        var banners = await _bannerService.GetAllBannersAsync(1, 10);

        return new BannerListModel().PrepareToGrid(null, banners, () =>
        {
            return banners.Select(banner =>
            {
                return banner.ToModel<BannerModel>();
            });
        });
    }

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
