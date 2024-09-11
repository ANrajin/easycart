using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.HireForms.Areas.Admin.Factories;
using Nop.Plugin.Widgets.HireForms.Areas.Admin.Models;
using Nop.Plugin.Widgets.HireForms.Domain;
using Nop.Plugin.Widgets.HireForms.Services;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Mvc.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms.Areas.Admin.Controllers
{
    public class HirePeriodController : BaseAdminController
    {
        #region Fields

        private readonly IHirePreriodService _hirePreriodService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IHirePeriodModelFactory _hirePeriodModelFactory;

        #endregion

        #region ctor

        public HirePeriodController(IHirePeriodModelFactory hirePeriodModelFactory,
            ILocalizationService localizationService,
            IHirePreriodService hirePreriodService,
            INotificationService notificationService)
        {
            _hirePeriodModelFactory = hirePeriodModelFactory;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _hirePreriodService = hirePreriodService;
        }

        #endregion

        #region List

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }
        public virtual async Task<IActionResult> List()
        {
            //prepare model
            var model = await _hirePeriodModelFactory.PrepareHirePeriodSearchModelAsync(new HirePeriodSearchModel());
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> List(HirePeriodSearchModel searchModel)
        {
            //prepare model
            var model = await _hirePeriodModelFactory.PrepareHirePeriodListModelAsync(searchModel);
            return Json(model);
        }

        #endregion

        #region Create / Edit / Delete

        public virtual async Task<IActionResult> Create()
        {
            //prepare model
            var model = await _hirePeriodModelFactory.PrepareHirePeriodModelAsync(new HirePeriodModel());

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Create(HirePeriodModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var hirePeriod = model.ToEntity<HirePeriod>();
                await _hirePreriodService.InsertHirePeriodAsync(hirePeriod);
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Widgets.HireForms.HirePeriods.HirePeriodAdded"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = hirePeriod.Id });
            }

            //prepare model
            model = await _hirePeriodModelFactory.PrepareHirePeriodModelAsync(model);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {
            //try to get a category with the specified id
            var hirePeriod = await _hirePreriodService.GetHirePeriodByIdAsync(id);
            if (hirePeriod == null)
                return RedirectToAction("List");

            //prepare model
            var model = await _hirePeriodModelFactory.PrepareHirePeriodModelAsync(new HirePeriodModel(), hirePeriod);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Edit(HirePeriodModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                //try to get a hire period with the specified id
                var hirePeriod = new HirePeriod();
                model.ToEntity(hirePeriod);

                if (hirePeriod == null)
                    return RedirectToAction("List");

                await _hirePreriodService.UpdateHirePeriodAsync(hirePeriod);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Widgets.HireForms.HirePeriods.HirePeriodUpdated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = hirePeriod.Id });
            }

            //prepare model
            model = await _hirePeriodModelFactory.PrepareHirePeriodModelAsync(model, null);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> DeleteSelected(ICollection<int> selectedIds)
        {
            if (selectedIds == null || selectedIds.Count == 0)
                return NoContent();

            // Get selected hire periods 
            var hirePeriods = await _hirePreriodService.GetHirePeriodsByIdsAsync(selectedIds);

            // Delete all the hire periods 
            await _hirePreriodService.DeleteHirePeriodsAsync(hirePeriods);

            return Json(new { Result = true });
        }

        #endregion
    }
}
