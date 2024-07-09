using AutoMapper;
using Nop.Core.Infrastructure.Mapper;

namespace Nop.Plugin.Widgets.TrendingProducts.Infrastructure.Mapper;
public class MapperConfiguration : Profile, IOrderedMapperProfile
{
    public MapperConfiguration()
    {
    }

    public int Order => 1;
}
