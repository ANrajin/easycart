using Nop.Core;
using Nop.Plugin.Widgets.HireForms.Areas.Admin.Models;
using Nop.Plugin.Widgets.HireForms.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms.Services
{
    public interface IHireRequestService
    {
        Task<IPagedList<HireRequest>> SearchRequestsAsync(string nameOrEmail = null,
            int hirePeriodId = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);

        Task<string> ExportRequestsToXmlAsync(IList<HireRequest> requests);

        //Task<byte[]> ExportRequestsToXlsxAsync(IList<HireRequest> requests);

        Task<IPagedList<HireRequest>> GetAllAsync(HireRequestSearchModel searchModel, int pageIndex = 0, int pageSize = int.MaxValue);

        Task<HireRequest> GetHireRequestByIdAsync(int id);

        Task<IList<HireRequest>> GetHireRequestsByIdsAsync(IList<int> ids);

        Task InsertHireRequestAsync(HireRequest hireRequest);

        Task UpdateHireRequestAsync(HireRequest hireRequest);

        Task DeleteHireRequestAsync(HireRequest hireRequest);

        Task DeleteHireRequestsAsync(IList<HireRequest> hireRequests);
    }
}