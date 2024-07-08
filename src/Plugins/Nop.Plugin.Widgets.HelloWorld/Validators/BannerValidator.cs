using FluentValidation;
using Nop.Plugin.Widgets.HelloWorld.Models;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.HelloWorld.Validators;
public class BannerValidator : BaseNopValidator<BannerModel>
{
    public BannerValidator()
    {
        RuleFor(x => x.ImageUrl)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull()
            .NotEmpty();
    }
}
