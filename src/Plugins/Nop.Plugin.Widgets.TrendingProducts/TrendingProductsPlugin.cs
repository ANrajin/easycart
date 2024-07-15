using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Plugin.Widgets.TrendingProducts.Components;
using Nop.Plugin.Widgets.TrendingProducts.Infrastructure.Securities;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Widgets.TrendingProducts
{
    public class TrendingProductsPlugin(
        ISettingService settingService,
        IStoreContext storeContext,
        IWebHelper webHelper,
        IPermissionService permissionService
        ) : BasePlugin, IAdminMenuPlugin, IWidgetPlugin
    {
        private readonly ISettingService _settingService = settingService;
        private readonly IStoreContext _storeContext = storeContext;
        private readonly IWebHelper _webHelper = webHelper;
        private readonly IPermissionService _permissionService = permissionService;

        public bool HideInWidgetList => false;

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.HomepageBeforeNews });
        }

        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(TrendingProductsViewComponent);
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/TrendingProducts/Configure";
        }

        public override async Task InstallAsync()
        {
            await _permissionService.InstallPermissionsAsync(new TrendingProductsPermissionProvider());
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            await _settingService.SaveSettingAsync(new TrendingProductsSetting(), storeScope);
            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            //uninstall permissions
            var permissionRecord = (await _permissionService.GetAllPermissionRecordsAsync())
                .FirstOrDefault(x => x.SystemName == TrendingProductsPermissionProvider.ViewTrendingProducts.SystemName);

            var listMappingCustomerRolePermissionRecord = await _permissionService.GetMappingByPermissionRecordIdAsync(permissionRecord.Id);

            foreach (var mappingCustomerPermissionRecord in listMappingCustomerRolePermissionRecord)
            {
                await _permissionService.DeletePermissionRecordCustomerRoleMappingAsync(
                        mappingCustomerPermissionRecord.PermissionRecordId,
                        mappingCustomerPermissionRecord.CustomerRoleId);
            }

            await _permissionService.DeletePermissionRecordAsync(permissionRecord);

            await _settingService.DeleteSettingAsync<TrendingProductsSetting>();
            await base.UninstallAsync();
        }

        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return;

            var menuItem = new SiteMapNode()
            {
                SystemName = "TrendingProductsPlugin",
                Title = "Trending Products",
                ControllerName = "TrendingProducts",
                ActionName = "List",
                IconClass = "fas fa-poll",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.ADMIN } },
            };
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "TrendingProductsPlugin");

            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Insert(2, menuItem);
        }
    }
}
