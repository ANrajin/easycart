using Nop.Plugin.Widgets.HelloWorld.Domain;

namespace Nop.Plugin.Widgets.HelloWorld.Services;
public interface IBannerService
{
    Task<Banner> GetBannerAsync();
    Task InsertBannerAsync(Banner banner);
}
