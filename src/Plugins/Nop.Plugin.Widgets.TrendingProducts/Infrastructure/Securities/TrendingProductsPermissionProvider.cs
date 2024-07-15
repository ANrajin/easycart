using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Security;
using Nop.Services.Security;

namespace Nop.Plugin.Widgets.TrendingProducts.Infrastructure.Securities;
public partial class TrendingProductsPermissionProvider : IPermissionProvider
{
    public static readonly PermissionRecord ViewTrendingProducts = new() { Name= "Admin Aria. View Trending Products", SystemName="ViewTrendingProducts", Category="Standard" };

    public IEnumerable<PermissionRecord> GetPermissions()
    {
        return new[]
        {
            ViewTrendingProducts
        };
    }

    public virtual HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions()
    {
        return new() { (NopCustomerDefaults.AdministratorsRoleName, new[] { ViewTrendingProducts }) };
    }
}
