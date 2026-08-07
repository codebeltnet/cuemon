using System.Collections.Generic;

namespace Cuemon.Xml.Assets;
public class WorldNode
{
    public string Code { get; set; }

    public string Name { get; set; }

    public string Kind { get; set; }

    public WorldLinks Links { get; set; }
}

public class WorldLinks
{
    public Link Self { get; set; }

    public List<Link> Children { get; set; }
}

public class Link
{
    public string Href { get; set; }

    public string Title { get; set; }
}
