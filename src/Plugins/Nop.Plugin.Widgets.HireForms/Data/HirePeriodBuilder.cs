using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.HireForms.Domain;

namespace Nop.Plugin.Widgets.HireForms.Data;
public class HirePeriodBuilder : NopEntityBuilder<HirePeriod>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(HirePeriod.Hour)).AsInt32()
            .NotNullable()
            .WithDefaultValue(0)
            .WithColumn(nameof(HirePeriod.TimeFrameId)).AsInt32();
    }
}
