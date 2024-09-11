using Nop.Plugin.Widgets.HireForms.Areas.Admin.Models;
using Nop.Plugin.Widgets.HireForms.Domain;
using Nop.Plugin.Widgets.HireForms.Services;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Models.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms.Areas.Admin.Factories
{
    public class HirePeriodModelFactory : IHirePeriodModelFactory
    {
        #region Fields

        private readonly IHirePreriodService _hirePreriodService;

        #endregion

        #region CTOR

        public HirePeriodModelFactory(IHirePreriodService hirePreriodService)
        {
            _hirePreriodService = hirePreriodService;
        }

        #endregion

        #region Methods
        public async Task<HirePeriodListModel> PrepareHirePeriodListModelAsync(HirePeriodSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //get hire periods
            var hirePeriods = await _hirePreriodService.GetAllHirePeriodsAsync(searchModel.Name,searchModel.Page - 1, searchModel.PageSize);

            //prepare grid model
            var model = await new HirePeriodListModel().PrepareToGridAsync(searchModel, hirePeriods, () =>
            {
                return hirePeriods.SelectAwait(async hirePeriod =>
                {
                    //fill in model values from the entity
                    var hirePeriodModel = hirePeriod.ToModel<HirePeriodModel>();
                    return await Task.FromResult(hirePeriodModel);
                });
            });

            return model;
        }

        public async Task<string> PrepareHirePeriodNameAsync(int hirePeriodId)
        {
            var hirePeriod = await _hirePreriodService.GetHirePeriodByIdAsync(hirePeriodId);
            //return hirePeriod.Name;
            return string.Empty;
        }

        public async Task<HirePeriodModel> PrepareHirePeriodModelAsync(HirePeriodModel model, HirePeriod hirePeriod = null)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (hirePeriod == null)
                return model;
            else
                return await Task.FromResult(hirePeriod.ToModel(model));
        }

        public async Task<HirePeriodSearchModel> PrepareHirePeriodSearchModelAsync(HirePeriodSearchModel searchModel = null)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            searchModel.SetGridPageSize();
            return await Task.FromResult(searchModel);
        }

        #endregion
    }
}
