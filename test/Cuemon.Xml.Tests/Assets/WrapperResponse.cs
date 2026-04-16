using System;
using System.Collections.Generic;
using System.Text;

namespace Cuemon.Xml.Assets;

public class WrapperResponse
{
    public WrapperResponse()
    {
    }

    public IEnumerable<LinkResponse> Links { get; set; } = new List<LinkResponse>()
    {
        {
            new LinkResponse("https://example.com", "self")
        },
        {
            new LinkResponse("https://example.com/other", "related")
        }
    };
}
