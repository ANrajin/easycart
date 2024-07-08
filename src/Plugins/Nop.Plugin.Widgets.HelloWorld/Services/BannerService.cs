using Nop.Data;
using Nop.Plugin.Widgets.HelloWorld.Domain;

namespace Nop.Plugin.Widgets.HelloWorld.Services;
public class BannerService(IRepository<Banner> bannerRepository) : IBannerService
{
    private readonly IRepository<Banner> _bannerRepository = bannerRepository;

    public async Task<Banner> GetBannerAsync()
    {
        return await _bannerRepository.GetByIdAsync(1);
    }

    public async Task InsertBannerAsync(Banner banner)
    {
        ArgumentNullException.ThrowIfNull(banner, nameof(banner));

        await _bannerRepository.InsertAsync(banner, false);
    }
}
