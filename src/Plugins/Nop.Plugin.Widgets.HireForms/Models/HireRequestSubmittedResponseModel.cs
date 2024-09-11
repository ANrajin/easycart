using Nop.Web.Framework.Models;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.HireForms.Models
{
    public record HireRequestSubmittedResponseModel : BaseNopModel
    {
        public HireRequestSubmittedResponseModel()
        {
            Errors = new List<string>();
        }
        public IList<string> Errors { get; set; }
        public bool Success { get; set; }
        public string ResponseMessage { get; set; }
    }
}
