using Nop.Core.Domain.Orders;
using Nop.Data;
using Nop.Plugin.Widgets.TrendingProducts.Dtos;

namespace Nop.Plugin.Widgets.TrendingProducts.Services;
public partial class TrendingProductService(IRepository<OrderItem> orderItemRepository) : ITrendingProductService
{
    private readonly IRepository<OrderItem> _orderItemRepository = orderItemRepository;

    public Task<IList<TrendingProductDto>> GetTrendingProducts()
    {
        throw new NotImplementedException();
    }
}
