using System.Xml.Serialization;

namespace Cuemon.Xml.Assets;

[XmlRoot("Link")]
public record LinkResponse
{
    public LinkResponse(string href, string rel)
    {
        Href = href;
        Rel = rel;
    }

    public string Href { get; set; }

    public string Rel { get; set; }

    public string Title { get; set; }

    public string Hreflang { get; set; }
}
