using Nop.Core;
using System;

namespace Nop.Plugin.Widgets.HireForms.Domain
{
    public class HireRequest : BaseEntity
    {
        public int CurrentProductId { get; set; }
        public int HirePrediodId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Quantity { get; set; }
    }
}
