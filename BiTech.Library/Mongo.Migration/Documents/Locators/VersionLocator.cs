using System;
using System.Collections.Generic;
using System.Linq;
using Mongo.Migration.Documents.Attributes;

namespace Mongo.Migration.Documents.Locators
{
    internal class VersionLocator : IVersionLocator
    {
        private IDictionary<Type, DocumentVersion> _versions;

        private IDictionary<Type, DocumentVersion> Versions
        {
            get
            {
                if (_versions == null)
                    LoadVersions();
                return _versions;
            }
        }

        public DocumentVersion? GetCurrentVersion(Type type)
        {
            if (!Versions.ContainsKey(type))
                return null;

            DocumentVersion value;
            Versions.TryGetValue(type, out value);
            return value;
        }

        public void LoadVersions()
        {
            List<aa> types = new List<aa>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var typesz = assembly.GetTypes();
                    foreach (var t in typesz)
                    {
                        var attributes = t.GetCustomAttributes(typeof(CurrentVersion), true);
                        if (attributes != null && attributes.Length > 0)
                        {
                            //migrationTypes.Add(type);
                            types.Add(new Locators.VersionLocator.aa() { Type = t, Attributes = attributes.Cast<CurrentVersion>() });
                        }
                    }
                }
                catch { }
            }

            //var types =
            //    from a in AppDomain.CurrentDomain.GetAssemblies()
            //    from t in a.GetTypes()
            //    let attributes = t.GetCustomAttributes(typeof(CurrentVersion), true)
            //    where attributes != null && attributes.Length > 0
            //    select new {Type = t, Attributes = attributes.Cast<CurrentVersion>()};

            var versions = new Dictionary<Type, DocumentVersion>();

            foreach (var type in types)
            {
                var version = type.Attributes.First().Version;
                versions.Add(type.Type, version);
            }

            _versions = versions;
        }

        private class aa {

            public Type Type { get; set; }
            public IEnumerable<CurrentVersion> Attributes { get; set; }
        }

    }
}