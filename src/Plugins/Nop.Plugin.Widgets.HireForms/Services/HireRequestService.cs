using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Widgets.HireForms.Areas.Admin.Models;
using Nop.Plugin.Widgets.HireForms.Domain;
using Nop.Services.ExportImport;
using Nop.Services.ExportImport.Help;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Nop.Plugin.Widgets.HireForms.Services
{
    public class HireRequestService : IHireRequestService
    {
        #region Fields

        private readonly IRepository<HireRequest> _hireRequestRepository;
        private readonly CatalogSettings _catalogSettings;

        #endregion

        #region CTOR

        public HireRequestService(CatalogSettings catalogSettings, IRepository<HireRequest> hireRequestRepository)
        {
            _catalogSettings = catalogSettings;
            _hireRequestRepository = hireRequestRepository;
        }

        #endregion

        #region Methods

        public virtual async Task<IPagedList<HireRequest>> SearchRequestsAsync(string nameOrEmail = null,
            int hirePeriodId = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = _hireRequestRepository.Table;

            if (nameOrEmail != null)
                query = query.Where(o => o.Name == nameOrEmail || o.Email == nameOrEmail);

            if (hirePeriodId > 0)
                query = query.Where(o => o.HirePrediodId == hirePeriodId)
                    .Select(o => o);

            //database layer paging
            return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
        }

        public virtual async Task<string> ExportRequestsToXmlAsync(IList<HireRequest> requests)
        {

            var settings = new XmlWriterSettings
            {
                Async = true,
                ConformanceLevel = ConformanceLevel.Auto
            };

            await using var stringWriter = new StringWriter();
            await using var xmlWriter = XmlWriter.Create(stringWriter, settings);

            await xmlWriter.WriteStartDocumentAsync();
            await xmlWriter.WriteStartElementAsync("Requests");
            await xmlWriter.WriteAttributeStringAsync("Version", NopVersion.CURRENT_VERSION);

            foreach (var request in requests)
            {
                await xmlWriter.WriteStartElementAsync("Requests");
                await xmlWriter.WriteStringAsync("RequestID", request.Id.ToString());
                await xmlWriter.WriteStringAsync("Name", request.Name);
                await xmlWriter.WriteStringAsync("Email", request.Email);
                await xmlWriter.WriteStringAsync("StartDate", request.StartDate);
                await xmlWriter.WriteStringAsync("EndDate", request.EndDate);
                await xmlWriter.WriteStringAsync("CreatedOnUtc", request.CreatedOn);
                await xmlWriter.WriteEndElementAsync();
            }

            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.WriteEndDocumentAsync();
            await xmlWriter.FlushAsync();
            return stringWriter.ToString();
        }


        //public virtual async Task<byte[]> ExportRequestsToXlsxAsync(IList<HireRequest> requests)
        //{
        //    //property array
        //    var properties = new[]
        //    {
        //        new PropertyByName<HireRequest>("RequestID", p => p.Id),
        //        new PropertyByName<HireRequest>("Name", p => p.Name),
        //        new PropertyByName<HireRequest>("Email", p => p.Email),
        //        new PropertyByName<HireRequest>("StartDate", p => p.StartDate),
        //        new PropertyByName<HireRequest>("EndDate", p => p.EndDate),
        //        new PropertyByName<HireRequest>("CreatedOnUtc", p => p.CreatedOn)
        //    };

        //    return await new PropertyManager<HireRequest>(properties, _catalogSettings).ExportToXlsxAsync(requests);
        //}


        public HireRequestService(IRepository<HireRequest> hireRequestRepository)
        {
            _hireRequestRepository = hireRequestRepository;
        }

        public async Task DeleteHireRequestAsync(HireRequest hireRequest)
        {
            await _hireRequestRepository.DeleteAsync(hireRequest);
        }

        public async Task DeleteHireRequestsAsync(IList<HireRequest> hireRequests)
        {
            if (hireRequests == null)
                throw new ArgumentNullException(nameof(hireRequests));
            foreach (var hireRequest in hireRequests)
                await DeleteHireRequestAsync(hireRequest);
        }

        public async Task<IPagedList<HireRequest>> GetAllAsync(HireRequestSearchModel searchModel, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var requests = await _hireRequestRepository.GetAllPagedAsync(query =>
                from h in query
                where h.Id > 0 // Example condition, you can modify this as needed
                    && (searchModel.HirePeriodId == 0 || h.HirePrediodId == searchModel.HirePeriodId)
                    && (string.IsNullOrEmpty(searchModel.CustomerNameOrEmail) ||
                        h.Name.Contains(searchModel.CustomerNameOrEmail) ||
                        h.Email.Contains(searchModel.CustomerNameOrEmail))
                        orderby h.Id descending
                select h
            );

            return requests;
        }


        public Task<HireRequest> GetHireRequestByIdAsync(int id)
        {
            if (id == 0) return null;
            else
                return _hireRequestRepository.GetByIdAsync(id);
        }

        public async Task<IList<HireRequest>> GetHireRequestsByIdsAsync(IList<int> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));
            return await _hireRequestRepository.Table.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task InsertHireRequestAsync(HireRequest hireRequest)
        {
            await _hireRequestRepository.InsertAsync(hireRequest);
        }

        public async Task UpdateHireRequestAsync(HireRequest hireRequest)
        {
            await _hireRequestRepository.UpdateAsync(hireRequest);
        }

        #endregion
    }
}
