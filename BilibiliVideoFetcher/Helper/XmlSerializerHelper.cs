using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BilibiliVideoFetcher.Helper
{
    public static class XmlSerializerHelper
    {

        private static ConcurrentDictionary<Type, XmlSerializer> _cache;
        private static XmlSerializerNamespaces _defaultNamespace;

        static XmlSerializerHelper()
        {
            _defaultNamespace = new XmlSerializerNamespaces();
            _defaultNamespace.Add(string.Empty, string.Empty);

            _cache = new ConcurrentDictionary<Type, XmlSerializer>();
        }


        private static XmlSerializer GetSerializer<T>()
        {
            var type = typeof(T);
            return _cache.GetOrAdd(type, XmlSerializer.FromTypes(new[] { type }).FirstOrDefault());
        }


        public static string XmlSerialize<T>(this T obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                GetSerializer<T>().Serialize(memoryStream, obj, _defaultNamespace);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        public static T XmlDeserialize<T>(this string xml)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                var obj = GetSerializer<T>().Deserialize(memoryStream);
                return obj == null ? default(T) : (T)obj;
            }
        }
        public static T XmlDeserialize<T>(this byte xml)
        {
            using (var memoryStream = new MemoryStream(xml))
            {
                var obj = GetSerializer<T>().Deserialize(memoryStream);
                return obj == null ? default(T) : (T)obj;
            }
        }
    }
}
