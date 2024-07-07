using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.HelloWorld.Domain;

namespace Nop.Plugin.Widgets.HelloWorld.Migrations;

[NopSchemaMigration("2024/07/07 08:40:55:1687541", "Widgets.HelloWorld base schema", MigrationProcessType.Installation)]
public class ShemaMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.TableFor<Banner>();
    }
}
