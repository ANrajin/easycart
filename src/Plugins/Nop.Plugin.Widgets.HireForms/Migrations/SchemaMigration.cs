using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.HireForms.Domain;

namespace Nop.Plugin.Widgets.HireForms.Migrations
{
    [NopMigration("2023/08/12 09:09:17:6455420", "Widget.HireForms base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        #region Methods
        public override void Up()
        {
            Create.TableFor<HirePeriod>();
            Create.TableFor<HireRequest>();
        }
        #endregion
    }
}