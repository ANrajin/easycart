using FluentValidation;
using Nop.Plugin.Widgets.TrendingProducts.Models;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.TrendingProducts.Validators;
public class ConfigurationValidator : BaseNopValidator<ConfigurationModel>
{
    public ConfigurationValidator()
    {
        RuleFor(x => x.FromDate)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.ToDate)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Count)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.SlidesToShow)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.SlidesToScroll)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.AutoPlaySpeed)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty();
    }
}
