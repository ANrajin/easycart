using FluentMigrator;
using Nop.Core;
using Nop.Data.Mapping;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.HireForms.Domain;

namespace Nop.Plugin.Widgets.HireForms.Migrations;

[NopMigration("2024/09/11 05:09:17:6455420", "Hire period table alter schema", MigrationProcessType.Update)]
public class AlterHirePeriodTable : Migration
{
    public static string TableName<T>() where T : BaseEntity
    {
        return NameCompatibilityManager.GetTableName(typeof(T));
    }

    public override void Down()
    {
    }

    public override void Up()
    {
        if (Schema.Table(TableName<HirePeriod>()).Exists())
        {
            Rename.Column("Name").OnTable(TableName<HirePeriod>()).To(nameof(HirePeriod.Hour));
            Alter.Column(nameof(HirePeriod.Hour)).OnTable(TableName<HirePeriod>()).AsInt32().NotNullable().WithDefaultValue(0);
        }

        if (!Schema.Table(TableName<HirePeriod>()).Column(nameof(HirePeriod.TimeFrameId)).Exists())
        {
            Alter.Table(TableName<HirePeriod>())
                .AddColumn(nameof(HirePeriod.TimeFrameId)).AsInt32().NotNullable().WithDefaultValue(0);
        }
    }
}
