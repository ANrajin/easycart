using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.HireForms.Domain;
using Nop.Plugin.Widgets.HireForms.Models;
using Nop.Plugin.Widgets.HireForms.Services;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Controllers;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms.Controllers
{
    public class HireFormController : BasePluginController
    {
        #region Fields

        private readonly IHireRequestService _hireRequestService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IDateTimeHelper _dateTimeHelper;

        #endregion

        #region CTOR

        public HireFormController(IHireRequestService hireRequestService,
            ILocalizationService localizationService,
            IWorkContext workContext, 
            IDateTimeHelper dateTimeHelper)
        {
            _hireRequestService = hireRequestService;
            _localizationService = localizationService;
            _workContext = workContext;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region Methods

        private void ParseDates(IFormCollection form, out DateTime? startDate, out DateTime? endDate)
        {
            startDate = null;
            endDate = null;
            var ctrlStartDate = form["HireForm_StartDate"].FirstOrDefault();
            var ctrlEndDate = form["HireForm_EndDate"].FirstOrDefault();
            try
            {
                startDate = DateTime.ParseExact(ctrlStartDate,
                    CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern,
                    CultureInfo.InvariantCulture);
                endDate = DateTime.ParseExact(ctrlEndDate,
                    CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern,
                    CultureInfo.InvariantCulture);
            }
            catch
            {
                // ignored
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public virtual async Task<IActionResult> HireForm_Details(IFormCollection form)
        {
            var response = new HireRequestSubmittedResponseModel();
            try
            {
                var hireFormModel = new HireFormModel
                {
                    Quantity = int.TryParse(form["HireForm_HireFormQuantity"].FirstOrDefault(), out int quantity) ? quantity : 0,
                    SelectedHirePeriodId = int.TryParse(form["HireForm_SelectedHirePeriodId"].FirstOrDefault(), out int selectedHirePeriodId) ? selectedHirePeriodId : 0,
                    Name = form["HireForm_Name"].FirstOrDefault(),
                    Email = form["HireForm_Email"].FirstOrDefault(),
                    Phone = form["HireForm_Phone"].FirstOrDefault(),
                    CurrentProductId = int.Parse(form["CurrentProductId"].FirstOrDefault())
                };
                ParseDates(form, out var sStartDate, out var sEndDate);
                if (!sStartDate.HasValue)
                {
                    response.Errors.Add(await _localizationService.GetResourceAsync("ShoppingCart.Rental.EnterStartDate"));
                }

                if (!sEndDate.HasValue)
                {
                    response.Errors.Add(await _localizationService.GetResourceAsync("ShoppingCart.Rental.EnterEndDate"));
                }

                if (sEndDate.Value.CompareTo(sEndDate.Value) > 0)
                {
                    response.Errors.Add(await _localizationService.GetResourceAsync("ShoppingCart.Rental.StartDateLessEndDate"));
                }
                var hireRequest = hireFormModel.ToEntity<HireRequest>();
                if (!response.Errors.Any())
                {
                    hireFormModel.StartDate = sStartDate.HasValue ? (DateTime?)_dateTimeHelper.ConvertToUtcTime(sStartDate.Value, await                _dateTimeHelper.GetCurrentTimeZoneAsync()) : null;
                    hireFormModel.EndDate = sEndDate.HasValue ? (DateTime?)_dateTimeHelper.ConvertToUtcTime(sEndDate.Value, await 
                        _dateTimeHelper.GetCurrentTimeZoneAsync()) : null;
                    hireRequest.CustomerId = (await _workContext.GetCurrentCustomerAsync()).Id;
                    hireRequest.CreatedOn = DateTime.UtcNow;
                    hireRequest.StartDate = sStartDate.Value.Date;
                    hireRequest.EndDate = sEndDate.Value.Date;
                    await _hireRequestService.InsertHireRequestAsync(hireRequest);
                    response.ResponseMessage = await _localizationService.GetResourceAsync("Plugins.Widgets.HireForms.Submission.Success");
                    response.Success = true;
                    return Json(response);
                }
                response.ResponseMessage = string.Join("<br/>", response.Errors);
                return Json(response);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                response.Errors.Add(await _localizationService.GetResourceAsync("Plugins.Widgets.HireForms.Public.RequestHire.ParseError"));
                response.ResponseMessage = await _localizationService.GetResourceAsync("Plugins.Widgets.HireForms.Public.RequestHire.ParseError");
                return Json(response);
            }
        }

        #endregion
    }
}
