using LinqToDB;
using Nop.Core;
using Nop.Data;
using Nop.Plugin.Widgets.HelloWorld.Domain;

namespace Nop.Plugin.Widgets.HelloWorld.Services;
public class BannerService(IRepository<Banner> bannerRepository) : IBannerService
{
    private readonly IRepository<Banner> _bannerRepository = bannerRepository;

    public async Task<IPagedList<Banner>> GetAllBannersAsync(int pageIndex, int pageSize)
    {
        var bannerList = await _bannerRepository.GetAllAsync(x => x.Where(c => c.ImageUrl > 0));

        var pagedList = new PagedList<Banner>(bannerList, pageIndex, pageSize, 10);

        return pagedList;
    }

    public async Task<Banner> GetBannerAsync()
    {
        return await _bannerRepository.GetByIdAsync(1);
    }

    public async Task InsertBannerAsync(Banner banner)
    {
        ArgumentNullException.ThrowIfNull(banner, nameof(banner));

        await _bannerRepository.InsertAsync(banner, false);
    }

    public async Task UpdateBannerAsync(Banner banner)
    {
        ArgumentNullException.ThrowIfNull(banner, nameof(banner));

        await _bannerRepository.UpdateAsync(banner, false);
    }
}
