---
uid: Cuemon.Extensions.Net.Http.HttpManagerFactory
example:
- *content
---

```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.Extensions.Net.Http;

namespace Cuemon.Extensions.Net.Http;

public class HttpManagerFactoryExample
{
    public async Task DemonstrateAsync()
    {
        var clientFactory = new HttpClientFactoryStub();

        var manager = HttpManagerFactory.CreateManager(clientFactory, "github");
        var response = await manager.HttpGetAsync(new Uri("https://api.github.com"));
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }

    private class HttpClientFactoryStub : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("HttpManagerFactoryExample");
            return client;
        }
    }
}
```
