using System.Xml.Linq;

namespace Monogame.Processing
{
    public class XML
    {
        public XML(XDocument document) => Document = document;

        public XDocument Document { get; }

        public static XML Parse(string xml) => new XML(XDocument.Parse(xml));

        public override string ToString() => Document?.ToString();
    }
}
