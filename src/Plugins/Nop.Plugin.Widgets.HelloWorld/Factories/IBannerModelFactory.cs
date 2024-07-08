using Nop.Plugin.Widgets.HelloWorld.Domain;
using Nop.Plugin.Widgets.HelloWorld.Models;

namespace Nop.Plugin.Widgets.HelloWorld.Factories;
public interface IBannerModelFactory
{
    Task<BannerListModel> PrepareBannerListModelAsync();
    Task<BannerModel> PrepareBannerModelAsync(BannerModel model, Banner banner);
}
