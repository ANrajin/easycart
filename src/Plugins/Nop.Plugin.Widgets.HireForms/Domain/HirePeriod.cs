using Nop.Core;

namespace Nop.Plugin.Widgets.HireForms.Domain
{
    public class HirePeriod : BaseEntity
    {
        public int Hour { get; set; }

        public int TimeFrameId { get; set; }
    }
}
