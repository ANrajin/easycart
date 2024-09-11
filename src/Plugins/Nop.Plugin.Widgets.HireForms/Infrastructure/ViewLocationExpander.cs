using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Widgets.HireForms.Infrastructure
{
    public class ViewLocationExpander : IViewLocationExpander
    {

        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.AreaName == "Admin")
            {
                viewLocations = new[] {
                    $"~/Plugins/Widgets.HireForms/Areas/Admin/Views/Shared/{{0}}.cshtml",
                    $"~/Plugins/Widgets.HireForms/Areas/Admin/Views/{{1}}/{{0}}.cshtml"
                }.Concat(viewLocations);
            }
            else
            {
                viewLocations = new[] {
                        $"~/Plugins/Widgets.HireForms/Views/{{1}}/{{0}}.cshtml",
                        $"~/Plugins/Widgets.HireForms/Views/Shared/{{0}}.cshtml"
                    }.Concat(viewLocations);
            }
            return viewLocations;
        }
    }
}
