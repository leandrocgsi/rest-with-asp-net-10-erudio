using System.Text;
using System.Xml.Serialization;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests.Tools
{
    public static class XmlHelper
    {
        public static StringContent SerializeToXml<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            using var stringWriter = new Utf8StringWriter();
            serializer.Serialize(stringWriter, obj, ns);

            return new StringContent(stringWriter.ToString(), Encoding.UTF8, "application/xml");
        }

        public static async Task<T?> ReadFromXmlAsync<T>(
            HttpResponseMessage response)
        {
            var serializer = new XmlSerializer(typeof(T));
            await using var stream = await response
                .Content.ReadAsStreamAsync();
            return (T?)serializer.Deserialize(stream);
        }

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
