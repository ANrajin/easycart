using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.HireForms.Models
{
    public record HireFormModel : BaseNopEntityModel
    {
        public HireFormModel()
        {
            AvailableHirePreriods = new List<SelectListItem>();
        }
        public int CurrentProductId { get; set; }
        public int Quantity { get; set; }
        public int SelectedHirePeriodId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string InfoBody { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IList<SelectListItem> AvailableHirePreriods { get; set; }
    }
}
