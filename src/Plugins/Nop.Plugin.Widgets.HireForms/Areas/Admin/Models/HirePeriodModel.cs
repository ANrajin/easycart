using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.HireForms.Areas.Admin.Models
{
    public record HirePeriodModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Plugins.Widgets.HireForms.HirePeriods.Model.Duration")]
        public int Hour { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.HireForms.HirePeriods.Model.TimeFrame")]
        public int TimeFrameId { get; set; }
    }
}
