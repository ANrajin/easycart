using Nop.Services.Plugins;

namespace Nop.Plugin.Widgets.Test;

public class TestPlugin:BasePlugin
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
