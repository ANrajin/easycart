using Nop.Core;
using Nop.Services.Plugins;

namespace Nop.Plugin.Misc.GoogleShoppingFeed;

public class GoogleShoppingFeedPlugin(IWebHelper webHelper) : BasePlugin
{
    private readonly IWebHelper _webHelper = webHelper;

    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/GoogleShoppingFeed/Configure";
    }

    public override async Task InstallAsync()
    {
        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await base.UninstallAsync();
    }
}
