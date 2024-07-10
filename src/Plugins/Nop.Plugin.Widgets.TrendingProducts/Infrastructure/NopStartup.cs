using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.TrendingProducts.Factories;
using Nop.Plugin.Widgets.TrendingProducts.Services;

namespace Nop.Plugin.Widgets.TrendingProducts.Infrastructure;
public class NopStartup : INopStartup
{
    public int Order => 1;

    public void Configure(IApplicationBuilder application)
    {
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationExpanders.Add(new ViewLocationExpander());
        });

        services.AddScoped<ITrendingProductService, TrendingProductService>();
        services.AddScoped<ITrendingProductFactory, TrendingProductFactory>();
    }
}
