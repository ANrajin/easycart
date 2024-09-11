using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.HireForms
{
    public class HireFormsSettings : ISettings
    {
        public string HireFormBottomInfo { get; set; }
        public string InfoHeader { get; set; }
        public string InfoBody { get; set; }
    }
}
