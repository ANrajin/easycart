using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Plugin.Widgets.HelloWorld.Components;
using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Web.Framework;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Widgets.HelloWorld;

public class HelloWorldPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
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

    public Task ManageSiteMapAsync(SiteMapNode rootNode)
    {
        var menuItem = new SiteMapNode()
        {
            SystemName = "HelloWorldPlugin",
            Title = "Hello World",
            ControllerName = "HelloWorldHome",
            ActionName = "Index",
            IconClass = "fab fa-slack-hash",
            Visible = true,
            RouteValues = new RouteValueDictionary() { { "area", AreaNames.ADMIN } },
        };
        var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "HelloWorldPlugin");
        if (pluginNode != null)
            pluginNode.ChildNodes.Add(menuItem);
        else
            rootNode.ChildNodes.Insert(2,menuItem);

        return Task.CompletedTask;
    }

    public override async Task UninstallAsync()
    {
        await base.UninstallAsync();
    }
}
