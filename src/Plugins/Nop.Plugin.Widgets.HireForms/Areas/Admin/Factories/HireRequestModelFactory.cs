using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Widgets.HireForms.Areas.Admin.Models;
using Nop.Plugin.Widgets.HireForms.Domain;
using Nop.Plugin.Widgets.HireForms.Services;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms.Areas.Admin.Factories
{
    public class HireRequestModelFactory : IHireRequestModelFactory
    {
        #region Fields

        private readonly IHireRequestService _hireRequestService;
        private readonly IHirePreriodService _hirePreriodService;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;

        #endregion

        #region CTOR

        public HireRequestModelFactory(IHireRequestService hireRequestService,
            IHirePreriodService hirePreriodService,
            ILocalizationService localizationService,
            IDateTimeHelper dateTimeHelper)
        {
            _hireRequestService = hireRequestService;
            _hirePreriodService = hirePreriodService;
            _localizationService = localizationService;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region Methods

        private async Task<IList<SelectListItem>> PrepareAvailablePeriodsAsync()
        {
            var periods = await (await _hirePreriodService.GetAllHirePeriodsAsync()).Select(x =>
            {
                return new SelectListItem
                {
                    //Text = x.Name,
                    Value = x.Id.ToString()
                };
            }).ToListAsync();
            periods.Insert(0, new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Admin.Common.All"),
                Value = "0"
            });
            return periods; 
        }

        public async Task<HireRequestListModel> PrepareHireRequestListModelAsync(HireRequestSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //get hire Requests
            var hireRequests = await _hireRequestService.GetAllAsync(searchModel);

            //prepare grid model
            var model = await new HireRequestListModel().PrepareToGridAsync(searchModel, hireRequests, () =>
            {
                return hireRequests.SelectAwait(async hireRequest =>
                {
                    //fill in model values from the entity
                    var hireRequstModel = hireRequest.ToModel<HireRequestModel>();
                    hireRequstModel.StartDate = await _dateTimeHelper.ConvertToUserTimeAsync(hireRequest.StartDate, DateTimeKind.Utc);
                    hireRequstModel.EndDate = await _dateTimeHelper.ConvertToUserTimeAsync(hireRequest.EndDate, DateTimeKind.Utc);
                    hireRequstModel.CreatedOn = await _dateTimeHelper.ConvertToUserTimeAsync(hireRequest.CreatedOn, DateTimeKind.Utc);
                    return await Task.FromResult(hireRequstModel);
                });
            });

            return model;
        }

        public async Task<HireRequestModel> PrepareHireRequestModelAsync(HireRequestModel model = null, HireRequest hireRequest = null)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (hireRequest == null)
                return model;
            else
                return await Task.FromResult(hireRequest.ToModel(model));
        }

        public async Task<HireRequestSearchModel> PrepareHireRequestSearchModelAsync(HireRequestSearchModel searchModel = null)
        {
            searchModel ??= new HireRequestSearchModel();
            searchModel.AvailableHirePeriods = await PrepareAvailablePeriodsAsync();
            return searchModel;
        }

        #endregion
    }
}
