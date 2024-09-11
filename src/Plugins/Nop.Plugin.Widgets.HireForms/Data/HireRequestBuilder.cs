using FluentMigrator.Builders.Create.Table;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.HireForms.Domain;

namespace Nop.Plugin.Widgets.HireForms.Data
{
    public class HireRequestBuilder : NopEntityBuilder<HireRequest>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(HireRequest.HirePrediodId))
                .AsInt32()
                .NotNullable()
                .ForeignKey<HirePeriod>()
                .OnDelete(System.Data.Rule.None);
        }
    }
}
