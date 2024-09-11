using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.HireForms.Areas.Admin.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Controllers;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms.Areas.Admin.Controllers
{
    public class HireRequestPluginController : BaseAdminController
    {

        #region Fields

        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        #endregion

        #region Ctor

        public HireRequestPluginController(ILanguageService languageService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            ISettingService settingService,
            IStoreContext storeContext)
        {
            _languageService = languageService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        #endregion

        #region Methods

        public async Task<IActionResult> Configure()
        {
            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var hireRequestSettings = await _settingService.LoadSettingAsync<HireFormsSettings>(storeScope);

            var model = new ConfigurationModel
            {
                InfoBody = hireRequestSettings.InfoBody,

            };

            //locales
            await AddLocalesAsync(_languageService, model.Locales, async (locale, languageId) =>
            {
                locale.InfoHeader = await _localizationService
                    .GetLocalizedSettingAsync(hireRequestSettings, x => x.InfoHeader, languageId, 0, false, false);
                locale.InfoBody = await _localizationService
                     .GetLocalizedSettingAsync(hireRequestSettings, x => x.InfoBody, languageId, 0, false, false);
            });

            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.InfoBody_OverrideForStore = await _settingService.SettingExistsAsync(hireRequestSettings, x => x.InfoBody, storeScope);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return await Configure();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var hireRequestSettings = await _settingService.LoadSettingAsync<HireFormsSettings>(storeScope);

            //save settings
            hireRequestSettings.InfoBody = model.InfoBody;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            await _settingService.SaveSettingOverridablePerStoreAsync(hireRequestSettings, x => x.InfoBody, model.InfoBody_OverrideForStore, storeScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            //localization. no multi-store support for localization yet.
            foreach (var localized in model.Locales)
            {
                await _localizationService.SaveLocalizedSettingAsync(hireRequestSettings,
                    x => x.InfoHeader, localized.LanguageId, localized.InfoHeader);
                await _localizationService.SaveLocalizedSettingAsync(hireRequestSettings,
                    x => x.InfoBody, localized.LanguageId, localized.InfoBody);
            }

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        #endregion
    }
}
