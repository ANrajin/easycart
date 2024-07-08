using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.HelloWorld.Domain;

namespace Nop.Plugin.Widgets.HelloWorld.Mappings.Builders;
public class BannerBuilder : NopEntityBuilder<Banner>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(Banner.AltText)).AsString(255).Nullable()
            .WithColumn(nameof(Banner.LinkText)).AsString(255).Nullable()
            .WithColumn(nameof(Banner.ImageUrl)).AsString(255).NotNullable();
    }
}
