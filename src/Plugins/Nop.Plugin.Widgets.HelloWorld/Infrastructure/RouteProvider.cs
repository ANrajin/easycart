using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;
using Nop.Web.Framework;

namespace Nop.Plugin.Widgets.HelloWorld.Infrastructure;

/// <summary>
/// Represents plugin route provider
/// </summary>
public class RouteProvider : IRouteProvider
{
    /// <summary>
    /// Register routes
    /// </summary>
    /// <param name="endpointRouteBuilder">Route builder</param>
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute(name: "Plugin.Widget.HelloWorldAdmin.Configure",
            pattern: "Admin/HelloWorldAdmin/Configure",
            defaults: new { controller = "HelloWorldAdmin", action = "Configure", area = AreaNames.ADMIN });

        endpointRouteBuilder.MapControllerRoute(name: "Plugin.Widget.HelloWorldHome.Index",
            pattern: "Admin/HelloWorldHome/",
            defaults: new { controller = "HelloWorldHome", action = "HelloWorldIndex", area = AreaNames.ADMIN });
    }

    /// <summary>
    /// Gets a priority of route provider
    /// </summary>
    public int Priority => 0;
}