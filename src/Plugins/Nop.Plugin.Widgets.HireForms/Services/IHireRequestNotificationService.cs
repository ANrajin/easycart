using Nop.Plugin.Widgets.HireForms.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms.Services
{
    public interface IHireRequestNotificationService
    {
        Task<IList<int>> SendHireRequestAdminNotificationAsync(HireRequest hireRequest);

        Task<IList<int>> SendHireRequestCustomerNotificationAsync(HireRequest hireRequest);
    }
}