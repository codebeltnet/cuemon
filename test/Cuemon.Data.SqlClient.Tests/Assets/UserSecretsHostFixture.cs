using System.IO;
using Cuemon.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Data.SqlClient.Assets;
public sealed class UserSecretsHostFixture : ManagedHostFixture
{
    public override void ConfigureHost(Test hostTest)
    {
        Validator.ThrowIfNull(hostTest);
        Validator.ThrowIfNotContainsType(hostTest, Arguments.ToArrayOf(typeof(HostTest<>)), $"{nameof(hostTest)} is not assignable from HostTest<T>.");

        Host = new HostBuilder()
            .ConfigureHostConfiguration(config => config.AddEnvironmentVariables("DOTNET_"))
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                    .AddEnvironmentVariables()
                    .AddUserSecrets<UserSecretsHostFixture>(true); // NET 6 consequence change (why not keep NET 5 behaviour?)

                ConfigureCallback(config.Build(), context.HostingEnvironment);
            })
            .ConfigureServices((context, services) =>
            {
                Configuration = context.Configuration;
                Environment = context.HostingEnvironment;
                ConfigureServicesCallback(services);
            }).Build();
    }
}
