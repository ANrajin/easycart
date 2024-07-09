using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Plugins;
using Nop.Web.Framework;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Widgets.TrendingProducts
{
    public class TrendingProductsPlugin(
        ISettingService settingService,
        IWebHelper webHelper
        ) : BasePlugin, IAdminMenuPlugin
    {
        protected readonly ISettingService _settingService = settingService;
        protected readonly IWebHelper _webHelper = webHelper;

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/TrendingProducts/Configure";
        }

        public override async Task InstallAsync()
        {
            await _settingService.SaveSettingAsync(new TrendingProductsSetting());
            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await _settingService.DeleteSettingAsync<TrendingProductsSetting>();
            await base.UninstallAsync();
        }

        public Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "TrendingProductsPlugin",
                Title = "Trending Products",
                ControllerName = "TrendingProducts",
                ActionName = "Index",
                IconClass = "fas fa-poll",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.ADMIN } },
            };
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "TrendingProductsPlugin");
            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Insert(2, menuItem);

            return Task.CompletedTask;
        }
    }
}
