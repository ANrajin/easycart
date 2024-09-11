using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.HireForms.Areas.Admin.Models
{
    public class ConfigurationModel : ILocalizedModel<ConfigurationModel.ConfigurationLocalizedModel>
    {
        public ConfigurationModel()
        {
            Locales = new List<ConfigurationLocalizedModel>();
        }
        public int ActiveStoreScopeConfiguration { get; set; }

        


        [NopResourceDisplayName("Plugins.Widgets.HireForms.Configure.InfoBody")]
        public string InfoBody { get; set; }
        public bool InfoBody_OverrideForStore { get; set; }

        public IList<ConfigurationLocalizedModel> Locales { get; set; }

        #region Nested class

        public partial class ConfigurationLocalizedModel : ILocalizedLocaleModel
        {
            public int LanguageId { get; set; }
            public string InfoHeader { get; set; }
            public string InfoBody { get; set; }
        }

        #endregion

    }
}
