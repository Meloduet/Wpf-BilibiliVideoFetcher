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

        /// <summary>
        /// 将对象序列化到流中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="stream"></param>
        public static void XmlSerializeTo<T>(this T obj, Stream stream)
        {
            GetSerializer<T>().Serialize(stream, obj, _defaultNamespace);
        }

        public static void XmlSerializeTo<T>(this T obj, string fileName)
        {
            using (var fs = File.OpenWrite(fileName))
            {
                XmlSerializeTo<T>(obj, fs);
                fs.Flush();
            }
        }

        public static string XmlSerialize<T>(this T obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                XmlSerializeTo<T>(obj, memoryStream);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        public static T XmlDeserialize<T>(this Stream stream)
        {
            var obj = GetSerializer<T>().Deserialize(stream);
            return obj == null ? default(T) : (T)obj;
        }

        public static T XmlDeserialize<T>(this string xmlString)
        {
            return XmlDeserialize<T>(Encoding.UTF8.GetBytes(xmlString));
        }

        public static T XmlDeserialize<T>(this byte[] buf)
        {
            using (var memoryStream = new MemoryStream(buf))
            {
                return XmlDeserialize<T>(memoryStream);
            }
        }
    }
}
