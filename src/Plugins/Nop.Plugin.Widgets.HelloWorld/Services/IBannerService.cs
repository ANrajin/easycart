using Nop.Core;
using Nop.Plugin.Widgets.HelloWorld.Domain;

namespace Nop.Plugin.Widgets.HelloWorld.Services;
public interface IBannerService
{
    Task<IPagedList<Banner>> GetAllBannersAsync(int pageIndex, int pageSize);
    Task<Banner> GetBannerAsync();
    Task InsertBannerAsync(Banner banner);
    Task UpdateBannerAsync(Banner banner);
}
