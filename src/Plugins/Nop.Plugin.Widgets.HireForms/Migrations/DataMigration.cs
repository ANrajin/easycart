using FluentMigrator;
using Nop.Core;
using Nop.Data.Mapping;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.HireForms.Domain;

namespace Nop.Plugin.Widgets.HireForms.Migrations
{
    [NopMigration("2023/09/05 01:01:58:3545210", "HireRequest DataTable Migration", MigrationProcessType.Update)]
    public class DataMigration : Migration
    {
        public override void Down()
        {
            
        }

        public static string TableName<T>() where T : BaseEntity
        {
            return NameCompatibilityManager.GetTableName(typeof(T));
        }

        public override void Up()
        {
            if (!Schema.Table(TableName<HireRequest>()).Column(nameof(HireRequest.CurrentProductId)).Exists())
            {
                Alter.Table(TableName<HireRequest>())
                    .AddColumn(nameof(HireRequest.CurrentProductId)).AsInt32().NotNullable().WithDefaultValue(0);
            }
        }
    }
}
