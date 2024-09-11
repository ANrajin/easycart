using Nop.Core;
using Nop.Plugin.Widgets.HireForms.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms.Services
{
    public interface IHirePreriodService
    {
        Task<IPagedList<HirePeriod>> GetAllHirePeriodsAsync(string periodName = "", int pageIndex = 0, int pageSize = int.MaxValue);

        Task<HirePeriod> GetHirePeriodByIdAsync(int id);

        Task<IList<HirePeriod>> GetHirePeriodsByIdsAsync(ICollection<int> ids);

        Task InsertHirePeriodAsync(HirePeriod hirePeriod);

        Task UpdateHirePeriodAsync(HirePeriod hirePeriod);

        Task DeleteHirePeriodsAsync(IList<HirePeriod> hirePeriods);
    }
}