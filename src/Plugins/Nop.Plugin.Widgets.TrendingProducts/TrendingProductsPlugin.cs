using Nop.Services.Plugins;

namespace Nop.Plugin.Widgets.TrendingProducts
{
    public class TrendingProductsPlugin:BasePlugin
    {
        public override async Task InstallAsync()
        {
            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await base.UninstallAsync();
        }
    }
}
