using Nop.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;

namespace Nop.Plugin.Widgets.TrendingProducts.Services;
public partial class TrendingProductService(
    IRepository<Product> productRepository,
    IRepository<OrderItem> orderItemRepository,
    IRepository<Order> orderRepository
    ) : ITrendingProductService
{
    private readonly IRepository<Product> _productRepository = productRepository;
    private readonly IRepository<OrderItem> _orderItemRepository = orderItemRepository;
    private readonly IRepository<Order> _orderRepository = orderRepository;

    public async Task<IList<Product>> GetTrendingProducts(TrendingProductsSetting setting, int storeId)
    {
        return await (from order in _orderRepository.Table
                    where order.CreatedOnUtc >= setting.FromDate && order.CreatedOnUtc <= setting.ToDate
                    && order.StoreId.Equals(1)
                    join orderItem in _orderItemRepository.Table on order.Id equals orderItem.Id
                    join product in _productRepository.Table.Where(p => p.Published && !p.Deleted)
                    on orderItem.ProductId equals product.Id
                    group new { 
                        ProductId = product.Id, 
                        ProductName = product.Name, 
                        ShortDesc = product.ShortDescription,
                        ProductSku = product.Sku
                    }
                    by new { product.Id, product.Name, product.ShortDescription, product.Sku } into productGroup
                    orderby productGroup.Count() descending
                    select new Product
                    {
                        Id = productGroup.Key.Id,
                        Name = productGroup.Key.Name,
                        ShortDescription = productGroup.Key.ShortDescription,
                        Sku = productGroup.Key.Sku
                    }).Take(setting.Count).ToListAsync();
    }
}
