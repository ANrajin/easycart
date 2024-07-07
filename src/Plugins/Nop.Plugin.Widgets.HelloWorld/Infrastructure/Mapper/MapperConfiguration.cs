using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Widgets.HelloWorld.Domain;
using Nop.Plugin.Widgets.HelloWorld.Models;

namespace Nop.Plugin.Widgets.HelloWorld.Infrastructure.Mapper;
public class MapperConfiguration : Profile, IOrderedMapperProfile
{
    public MapperConfiguration()
    {
        CreateMap<BannerModel, Banner>();
    }

    public int Order => 1;
}
