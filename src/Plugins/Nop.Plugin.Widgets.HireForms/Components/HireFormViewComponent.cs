using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Plugin.Widgets.HireForms.Models;
using Nop.Plugin.Widgets.HireForms.Services;
using Nop.Services.Localization;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms.Components
{
    public class HireFormViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IHirePreriodService _hirePreriodService;
        private readonly ILocalizationService _localizationService;
        private readonly HireFormsSettings _hireFormsSettings;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;

        #endregion

        #region CTOR

        public HireFormViewComponent(IHirePreriodService hirePreriodService,
            ILocalizationService localizationService,
            HireFormsSettings hireFormsSettings,
            IStoreContext storeContext,
            IWorkContext workContext)
        {
            _hirePreriodService = hirePreriodService;
            _localizationService = localizationService;
            _hireFormsSettings = hireFormsSettings;
            _storeContext = storeContext;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var productDetailsModel = (ProductDetailsModel)additionalData;
            if (!productDetailsModel.ProductPrice.CallForPrice)
                return Content(string.Empty);
            var store = await _storeContext.GetCurrentStoreAsync();
            var model = new HireFormModel
            {
                CurrentProductId = productDetailsModel.Id,
                Quantity = 1,
                EndDate = DateTime.Now,
                StartDate = DateTime.Now,
                InfoBody = await _localizationService.GetLocalizedSettingAsync(_hireFormsSettings,
                    x => x.InfoBody, (await _workContext.GetWorkingLanguageAsync()).Id, store.Id),
                AvailableHirePreriods = await (await _hirePreriodService.GetAllHirePeriodsAsync()).Select(x =>
                {
                    return new SelectListItem
                    {
                        //Text = x.Name,
                        Value = x.Id.ToString()
                    };
                }).ToListAsync()
            };
            model.AvailableHirePreriods.Insert(0, new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Plugins.Widgets.HireForms.HirePeriods.SelectHirePeriod"),
                Value = "0"
            });
            
            return View(model);
        }

        #endregion
    }
}
