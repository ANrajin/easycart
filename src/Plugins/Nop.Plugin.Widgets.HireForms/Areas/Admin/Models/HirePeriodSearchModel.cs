using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.HireForms.Areas.Admin.Models
{
    public record HirePeriodSearchModel : BaseSearchModel
    {
        [NopResourceDisplayName("Plugins.Widgets.HireForms.HirePeriods.HirePeriodSearchModel.Name")]
        public string Name { get; set; }
    }
}
