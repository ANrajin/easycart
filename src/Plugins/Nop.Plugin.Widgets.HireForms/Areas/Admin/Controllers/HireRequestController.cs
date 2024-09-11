using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.HireForms.Areas.Admin.Factories;
using Nop.Plugin.Widgets.HireForms.Areas.Admin.Models;
using Nop.Plugin.Widgets.HireForms.Domain;
using Nop.Plugin.Widgets.HireForms.Services;
using Nop.Services.ExportImport;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms.Areas.Admin.Controllers
{
    public class HireRequestController : BaseAdminController
    {
        #region Fields

        private readonly IHireRequestModelFactory _hireRequestModelFactory;
        private readonly IHirePeriodModelFactory _hirePeriodModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IHireRequestService _hireRequestService;
        private readonly INotificationService _notificationService;
        private readonly IExportManager _exportManager;
        private readonly IDateTimeHelper _dateTimeHelper;

        #endregion

        #region CTOR

        public HireRequestController(IHireRequestModelFactory hireRequestModelFactory,
            IHirePeriodModelFactory hirePeriodModelFactory,
            ILocalizationService localizationService,
            IHireRequestService hireRequestService,
            INotificationService notificationService,
            IExportManager exportManager,
            IDateTimeHelper dateTimeHelper)
        {
            _hireRequestModelFactory = hireRequestModelFactory;
            _hirePeriodModelFactory = hirePeriodModelFactory;
            _localizationService = localizationService;
            _hireRequestService = hireRequestService;
            _notificationService = notificationService;
            _exportManager = exportManager;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region Methods

        #region Export / Import

        [HttpPost, ActionName("ExportXml")]
        [FormValueRequired("exportxml-all")]
        public virtual async Task<IActionResult> ExportXmlAll(HireRequestSearchModel model)
        {
            //load requests
            var requests = await _hireRequestService.SearchRequestsAsync(nameOrEmail: model.CustomerNameOrEmail, hirePeriodId: model.HirePeriodId);

            //ensure that we at least one request selected
            if (!requests.Any())
            {
                _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Plugins.Widgets.HireForms.HireRequests.NoRequests"));
                return RedirectToAction("List");
            }

            try
            {
                var xml = await _hireRequestService.ExportRequestsToXmlAsync(requests);
                return File(Encoding.UTF8.GetBytes(xml), MimeTypes.ApplicationXml, "requests.xml");
            }
            catch (Exception exc)
            {
                await _notificationService.ErrorNotificationAsync(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> ExportXmlSelected(string selectedIds)
        {
            var requests = new List<HireRequest>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();
                requests.AddRange(await (await _hireRequestService.GetHireRequestsByIdsAsync(ids)).ToListAsync());
            }

            try
            {
                var xml = await _hireRequestService.ExportRequestsToXmlAsync(requests);
                return File(Encoding.UTF8.GetBytes(xml), MimeTypes.ApplicationXml, "requests.xml");
            }
            catch (Exception exc)
            {
                await _notificationService.ErrorNotificationAsync(exc);
                return RedirectToAction("List");
            }
        }

        //[HttpPost, ActionName("ExportExcel")]
        //[FormValueRequired("exportexcel-all")]
        //public virtual async Task<IActionResult> ExportExcelAll(HireRequestSearchModel model)
        //{

        //    //load requests
        //    var requests = await _hireRequestService.SearchRequestsAsync(nameOrEmail: model.CustomerNameOrEmail, hirePeriodId: model.HirePeriodId);

        //    //ensure that we at least one request selected
        //    if (!requests.Any())
        //    {
        //        _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Plugins.Widgets.HireForms.HireRequests.NoRequests"));
        //        return RedirectToAction("List");
        //    }

        //    try
        //    {
        //        var bytes = await _hireRequestService.ExportRequestsToXlsxAsync(requests);
        //        return File(bytes, MimeTypes.TextXlsx, "requests.xlsx");
        //    }
        //    catch (Exception exc)
        //    {
        //        await _notificationService.ErrorNotificationAsync(exc);
        //        return RedirectToAction("List");
        //    }
        //}

        //[HttpPost]
        //public virtual async Task<IActionResult> ExportExcelSelected(string selectedIds)
        //{
        //    var requests = new List<HireRequest>();
        //    if (selectedIds != null)
        //    {
        //        var ids = selectedIds
        //            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //            .Select(x => Convert.ToInt32(x))
        //            .ToArray();
        //        requests.AddRange(await (await _hireRequestService.GetHireRequestsByIdsAsync(ids)).ToListAsync());
        //    }

        //    try
        //    {
        //        var bytes = await _hireRequestService.ExportRequestsToXlsxAsync(requests);
        //        return File(bytes, MimeTypes.TextXlsx, "requests.xlsx");
        //    }
        //    catch (Exception exc)
        //    {
        //        await _notificationService.ErrorNotificationAsync(exc);
        //        return RedirectToAction("List");
        //    }
        //}


        #endregion

        #region List
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("List");
        }
        public async Task<IActionResult> List()
        {
            var model = await _hireRequestModelFactory.PrepareHireRequestSearchModelAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> HireRequests(HireRequestSearchModel searchModel)
        {
            var model = await _hireRequestModelFactory.PrepareHireRequestListModelAsync(searchModel);
            return Json(model);
        }

        #endregion

        #region Create / Edit / Delete

        public virtual async Task<IActionResult> Create()
        {
            //prepare model
            var model = await _hireRequestModelFactory.PrepareHireRequestModelAsync(new HireRequestModel());

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Create(HireRequestModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var hireRequest = model.ToEntity<HireRequest>();
                await _hireRequestService.InsertHireRequestAsync(hireRequest);
                model.Id = hireRequest.Id;
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Catalog.Categories.Added"));

                if (!continueEditing)
                    return RedirectToAction("List", "HireRequest");
                return RedirectToAction("Edit", new { id = hireRequest.Id });
            }

            //prepare model
            model = await _hireRequestModelFactory.PrepareHireRequestModelAsync(model);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {

            //try to get a hireRequest with the specified id
            var hireRequest = await _hireRequestService.GetHireRequestByIdAsync(id);

            // get hire peirod name from hirerequest.hireperiodid
            var hirePeriodName = await _hirePeriodModelFactory.PrepareHirePeriodNameAsync(hireRequest.HirePrediodId);

            if (hireRequest == null)
                return RedirectToAction("List");

            //prepare model
            var model = hireRequest.ToModel<HireRequestModel>();
            model = await _hireRequestModelFactory.PrepareHireRequestModelAsync(model);
            model.HirePeriodName = hirePeriodName;
            model.StartDate = await _dateTimeHelper.ConvertToUserTimeAsync(hireRequest.StartDate, DateTimeKind.Utc);
            model.EndDate = await _dateTimeHelper.ConvertToUserTimeAsync(hireRequest.EndDate, DateTimeKind.Utc);
            model.CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(hireRequest.CreatedOn, DateTimeKind.Utc);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Edit(HireRequestModel model, bool continueEditing)
        {
            //try to get a hireRequest with the specified id
            var hireRequest = await _hireRequestService.GetHireRequestByIdAsync(model.Id);
            if (hireRequest == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                hireRequest.Name = model.Name;
                hireRequest.Email = model.Email;
                hireRequest.Phone = model.Phone;
                hireRequest.Quantity = model.Quantity;
                hireRequest.StartDate = model.StartDate;
                hireRequest.EndDate = model.EndDate;

                await _hireRequestService.UpdateHireRequestAsync(hireRequest);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Catalog.Categories.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List", "HireRequest");

                return RedirectToAction("Edit", new { id = hireRequest.Id });
            }

            //prepare model
            model = await _hireRequestModelFactory.PrepareHireRequestModelAsync(model);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {
            //try to get a hireRequest with the specified id
            var hireRequest = await _hireRequestService.GetHireRequestByIdAsync(id);
            if (hireRequest == null)
                return RedirectToAction("List");

            await _hireRequestService.DeleteHireRequestAsync(hireRequest);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Catalog.Categories.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual async Task<IActionResult> DeleteSelectedHireRequests(ICollection<int> selectedIds)
        {
            if (selectedIds == null || selectedIds.Count == 0)
                return NoContent();

            await _hireRequestService.DeleteHireRequestsAsync(await (await _hireRequestService.GetHireRequestsByIdsAsync(selectedIds.ToArray())).ToListAsync());

            return Json(new { Result = true });
        }
        #endregion

        #endregion
    }
}