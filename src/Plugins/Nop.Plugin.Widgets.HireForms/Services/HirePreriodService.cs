using Nop.Core;
using Nop.Data;
using Nop.Plugin.Widgets.HireForms.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.HireForms.Services
{
    public class HirePreriodService : IHirePreriodService
    {
        #region Fields

        private readonly IRepository<HirePeriod> _hirePeriodRepository;

        #endregion

        #region CTOR
        public HirePreriodService(IRepository<HirePeriod> hirePeriodRepository)
        {
            _hirePeriodRepository = hirePeriodRepository;
        }

        #endregion

        #region Methods

        private async Task DeleteHirePeriodAsync(HirePeriod hirePeriod)
        {
            await _hirePeriodRepository.DeleteAsync(hirePeriod);
        }

        public async Task DeleteHirePeriodsAsync(IList<HirePeriod> hirePeriods)
        {
            if (hirePeriods == null)
                throw new ArgumentNullException(nameof(hirePeriods));
            foreach (var hirePeriod in hirePeriods)
                await DeleteHirePeriodAsync(hirePeriod);
        }


        public async Task<IPagedList<HirePeriod>> GetAllHirePeriodsAsync(string periodName = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            return await _hirePeriodRepository.GetAllPagedAsync(query =>
            {
                var q = from h in query
                        where string.IsNullOrEmpty(periodName)
                select h;
                return q;
            },pageIndex,pageSize);
        }

        public async Task<HirePeriod> GetHirePeriodByIdAsync(int id)
        {
            if (id == 0) return null;
            else
                return await _hirePeriodRepository.GetByIdAsync(id);
        }

        public async Task<IList<HirePeriod>> GetHirePeriodsByIdsAsync(ICollection<int> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));
            return await _hirePeriodRepository.Table.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task InsertHirePeriodAsync(HirePeriod hirePeriod)
        {
            await _hirePeriodRepository.InsertAsync(hirePeriod);
        }

        public async Task UpdateHirePeriodAsync(HirePeriod hirePeriod)
        {
            await _hirePeriodRepository.UpdateAsync(hirePeriod);
        }

        #endregion
    }
}
