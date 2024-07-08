using Microsoft.AspNetCore.Mvc.Razor;
using Nop.Core.Infrastructure;
using Nop.Web.Framework.Themes;
using Nop.Web.Framework;

namespace Nop.Plugin.Widgets.HelloWorld.Infrastructure;

/// <summary>
/// Specifies the contracts for a view location expander that is used by Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine instances to determine search paths for a view.
/// </summary>
public class ViewLocationExpander : IViewLocationExpander
{
    protected const string THEME_KEY = "nop.themename";

    /// <summary>
    /// Invoked by a <see cref="T:Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine" /> to determine the values that would be consumed by this instance
    /// of <see cref="T:Microsoft.AspNetCore.Mvc.Razor.IViewLocationExpander" />. The calculated values are used to determine if the view location
    /// has changed since the last time it was located.
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Razor.ViewLocationExpanderContext" /> for the current view location
    /// expansion operation.</param>
    public void PopulateValues(ViewLocationExpanderContext context)
    {
        //no need to add the themeable view locations at all as the administration should not be themeable anyway
        if (context.AreaName?.Equals(AreaNames.ADMIN) ?? false)
            return;

        context.Values[THEME_KEY] = EngineContext.Current.Resolve<IThemeContext>().GetWorkingThemeNameAsync().Result;
    }

    /// <summary>
    /// Invoked by a <see cref="T:Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine" /> to determine potential locations for a view.
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Razor.ViewLocationExpanderContext" /> for the current view location
    /// expansion operation.</param>
    /// <param name="viewLocations">The sequence of view locations to expand.</param>
    /// <returns>A list of expanded view locations.</returns>
    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
        IEnumerable<string> viewLocations)
    {
        context.Values.TryGetValue(THEME_KEY, out string theme);

        if (theme is (null or "DefaultClean"))
        {
            viewLocations = new[] {
                $"/Plugins/Widgets.HelloWorld/Views/{{1}}/{{0}}.cshtml",
                $"/Plugins/Widgets.HelloWorld/Views/Shared/{{0}}.cshtml",
            }.Concat(viewLocations);
        }
        else
        {
            viewLocations = new[] {
                $"/Plugins/Widgets.HelloWorld/Themes/{theme}/Views/{{1}}/{{0}}.cshtml",
                $"/Plugins/Widgets.HelloWorld/Themes/{theme}/Views/Shared/{{0}}.cshtml",
            }.Concat(viewLocations);
        }

        return viewLocations;
    }
}