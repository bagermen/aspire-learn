using Aspire.Hosting;
using DevProxy.Hosting;

namespace Aspire.Tests;

public class TestingAspireAppHost() : DistributedApplicationFactory(typeof(Projects.Aspire_AppHost)), IAsyncLifetime
{
    public async ValueTask InitializeAsync() => await StartAsync();

    protected override void OnBuilding(DistributedApplicationBuilder builder)
    {
        base.OnBuilding(builder);

        builder.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        var api = builder.CreateResourceBuilder(
            builder.Resources.OfType<ProjectResource>().First(res => res.Name == "aspire-api")
        );

        var api2 = builder.CreateResourceBuilder(
            builder.Resources.OfType<ProjectResource>().First(res => res.Name == "aspire-api2")
        );

        var devProxy = builder.AddDevProxyContainer("devproxy")
            .WithBindMount(Path.GetFullPath(".devproxy/cert", Directory.GetCurrentDirectory()), "/home/devproxy/.config/dev-proxy/rootCert")
            .WithBindMount(Path.GetFullPath(".devproxy/config", Directory.GetCurrentDirectory()), "/config")
            .WithConfigFile("./devproxy.json")
            .WithUrlsToWatch(() => [
                $"{api.GetEndpoint("https").Url}/*",
            ]);

        api2
            .WaitFor(devProxy)
            .WithEnvironment("https_proxy", devProxy.GetEndpoint(DevProxyResource.ProxyEndpointName));

    }
}
