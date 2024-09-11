using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.HireForms.Areas.Admin.Models
{
    public record HireRequestSearchModel : BaseSearchModel
    {
        public HireRequestSearchModel()
        {
            AvailableHirePeriods = new List<SelectListItem>();
        }
        [NopResourceDisplayName("Plugins.Widgets.HireForms.Table.HireRequest.Search.HirePeriodId")]
        public int HirePeriodId { get; set; }
        public IList<SelectListItem> AvailableHirePeriods { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.HireForms.Table.HireRequest.Search.EamilOrName")]
        public string CustomerNameOrEmail { get; set; }
    }
}
