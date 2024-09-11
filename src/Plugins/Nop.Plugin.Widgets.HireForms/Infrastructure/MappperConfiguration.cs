using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Widgets.HireForms.Areas.Admin.Models;
using Nop.Plugin.Widgets.HireForms.Domain;
using Nop.Plugin.Widgets.HireForms.Models;

namespace Nop.Plugin.Widgets.HireForms.Infrastructure
{
    public class MappperConfiguration : Profile, IOrderedMapperProfile
    {
        public MappperConfiguration()
        {
            CreateMap<HireRequest, HireRequestModel>();
            CreateMap<HireRequestModel, HireRequest>();
            CreateMap<HireFormModel, HireRequest>()
                .ForMember(entity => entity.HirePrediodId, options => options.MapFrom(x => x.SelectedHirePeriodId));
            CreateMap<HirePeriod, HirePeriodModel>();
            CreateMap<HirePeriodModel, HirePeriod>();
        }
        public int Order => 100;
    }
}
