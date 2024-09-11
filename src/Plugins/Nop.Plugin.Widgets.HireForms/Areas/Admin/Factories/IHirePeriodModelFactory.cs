using Nop.Plugin.Widgets.HireForms.Areas.Admin.Models;
using Nop.Plugin.Widgets.HireForms.Domain;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms.Areas.Admin.Factories
{
    public interface IHirePeriodModelFactory
    {

        Task<string> PrepareHirePeriodNameAsync(int hirePeriodId);
        Task<HirePeriodListModel> PrepareHirePeriodListModelAsync(HirePeriodSearchModel searchModel);

        Task<HirePeriodModel> PrepareHirePeriodModelAsync(HirePeriodModel model, HirePeriod hirePeriod = null);

        Task<HirePeriodSearchModel> PrepareHirePeriodSearchModelAsync(HirePeriodSearchModel searchModel = null);

    }
}