using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;

namespace Nop.Plugin.Widgets.HireForms.Areas.Admin.Models
{
    public record HireRequestModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Plugins.Widgets.HireForms.HireRequests.Edit.HirePeriodName")]
        public string HirePeriodName { get; set; }
        public int HirePrediodId { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.HireForms.HireRequests.Edit.CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.HireForms.HireRequests.Edit.StartDate")]
        public DateTime StartDate { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.HireForms.HireRequests.Edit.EndDate")]
        public DateTime EndDate { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.HireForms.HireRequests.Edit.Name")]
        public string Name { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.HireForms.HireRequests.Edit.Email")]
        public string Email { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.HireForms.HireRequests.Edit.Phone")]
        public string Phone { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.HireForms.HireRequests.Edit.Quantity")]
        public int Quantity { get; set; }
    }
}
