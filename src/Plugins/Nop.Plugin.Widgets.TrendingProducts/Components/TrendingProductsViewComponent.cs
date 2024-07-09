﻿using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.TrendingProducts.Components;
public class TrendingProductsViewComponent : NopViewComponent
{

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        return View();
    }
}
