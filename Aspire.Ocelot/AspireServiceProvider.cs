using Microsoft.Extensions.ServiceDiscovery;
using Ocelot.Configuration;
using Ocelot.ServiceDiscovery.Providers;
using Ocelot.Values;

namespace Aspire.Ocelot;

public class AspireServiceProvider(
    IServiceProvider serviceProvider,
    ServiceProviderConfiguration config,
    DownstreamRoute downstreamRoute
) : IServiceDiscoveryProvider
{
    private readonly ServiceEndpointResolver _endpointResolver = serviceProvider.GetRequiredService<ServiceEndpointResolver>();
    public async Task<List<Service>> GetAsync()
    {
        var result = await _endpointResolver.GetEndpointsAsync(
            $"{downstreamRoute.DownstreamScheme}://{downstreamRoute.ServiceName}",
            CancellationToken.None
        );

        return new(result.Endpoints.Select((sendpoint, idx) =>
        {
            var endPoint = new Uri(sendpoint.EndPoint.ToString()!);

            return new Service(
                name: downstreamRoute.ServiceName,
                hostAndPort: new ServiceHostAndPort(endPoint.Host, endPoint.Port),
                id: $"{downstreamRoute.ServiceName}-{idx}",
                version: "1.0",
                tags: new string[] { "downstream" }
            );
        })
        );
    }
}
