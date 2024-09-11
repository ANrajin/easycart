using Nop.Plugin.Widgets.HireForms.Areas.Admin.Models;
using Nop.Plugin.Widgets.HireForms.Domain;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms.Areas.Admin.Factories
{
    public interface IHireRequestModelFactory
    {
        Task<HireRequestListModel> PrepareHireRequestListModelAsync(HireRequestSearchModel searchModel);

        Task<HireRequestSearchModel> PrepareHireRequestSearchModelAsync(HireRequestSearchModel searchModel = null);

        Task<HireRequestModel> PrepareHireRequestModelAsync(HireRequestModel model = null, HireRequest hireRequest = null);
    }
}