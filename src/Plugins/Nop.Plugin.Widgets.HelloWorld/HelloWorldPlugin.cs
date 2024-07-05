using Nop.Core;
using Nop.Plugin.Widgets.HelloWorld.Components;
using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.HelloWorld;

public class HelloWorldPlugin : BasePlugin, IWidgetPlugin
{
    private readonly IWebHelper _webHelper;

    public HelloWorldPlugin(IWebHelper webHelper)
    {
        _webHelper = webHelper;
    }

    public bool HideInWidgetList => false;

    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/HelloWorldAdmin/Configure";
    }

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(HelloWorldViewComponent);
    }

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.HomepageBeforeNews });
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
