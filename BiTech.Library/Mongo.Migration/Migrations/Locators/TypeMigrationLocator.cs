using System;
using System.Linq;
using Mongo.Migration.Extensions;

namespace Mongo.Migration.Migrations.Locators
{
    internal class TypeMigrationLocator : MigrationLocator
    {
        public override void LoadMigrations()
        {
            var migrationTypes = new System.Collections.Generic.List<Type>();
            foreach (var assembly in Assemblies)
            {
                try
                {
                    var types = assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (typeof(IMigration).IsAssignableFrom(type) && !type.IsAbstract)
                        {
                            migrationTypes.Add(type);
                        }
                    }
                }
                catch { }
            }

            //var migrationTypes =
            //    (from assembly in Assemblies
            //    from type in assembly.GetTypes()
            //    where typeof(IMigration).IsAssignableFrom(type) && !type.IsAbstract
            //    select type).Distinct();
            //Migrations = migrationTypes.Select(t => (IMigration) Activator.CreateInstance(t)).ToMigrationDictionary();

            Migrations = migrationTypes.Select(t => (IMigration)Activator.CreateInstance(t)).ToMigrationDictionary();
        }
    }
}