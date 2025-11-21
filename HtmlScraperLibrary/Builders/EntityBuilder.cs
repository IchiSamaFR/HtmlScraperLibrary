using HtmlScraperLibrary.Entities;
using System.Xml.Linq;
using System.Reflection;

namespace HtmlScraperLibrary.Builders
{
    public class EntityBuilder
    {
        private const string EntitySuffix = "Entity";
        private static readonly Dictionary<string, Type> _entityTypes;

        static EntityBuilder()
        {
            _entityTypes = Assembly.GetAssembly(typeof(AEntity))!
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(AEntity).IsAssignableFrom(t))
                .ToDictionary(
                    t => t.Name.EndsWith(EntitySuffix) ? t.Name[..^EntitySuffix.Count()] : t.Name,
                    t => t,
                    StringComparer.OrdinalIgnoreCase
                );
        }

        public static AEntity? BuildFromFilePath(string path)
        {
            var element = XElement.Load(path);
            return BuildFromXml(element);
        }
        public static AEntity? BuildFromXml(XElement element)
        {
            if (_entityTypes.TryGetValue(element.Name.LocalName, out var type))
            {
                // Cherche un constructeur prenant un XElement
                var ctor = type.GetConstructor(new[] { typeof(XElement) });
                if (ctor != null)
                {
                    return (AEntity)ctor.Invoke(new object[] { element });
                }
            }
            return null;
        }
    }
}