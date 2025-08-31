using Aspire.Common;
using Aspire.Hosting.Yarp;
using Aspire.Hosting.Yarp.Transforms;
using Scalar.Aspire;
using Yarp.ReverseProxy.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

#region postgres

var postgresPwd = builder.AddParameter("PostgresPassword", secret: true);
var postgres = builder.AddPostgres("postgres", password: postgresPwd)
    .WithPgWeb()
    .WithImageTag("17.5")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var dbStore = postgres.AddDatabase(Constants.DbConnection, "store");

#endregion

#region aspire-api
var api = builder.AddProject<Projects.Aspire_Api>("aspire-api")
    .WithReference(dbStore).WaitFor(dbStore)
;
#endregion

#region scalar

var scalar = builder.AddScalarApiReference(options =>
{
    options.WithTheme(ScalarTheme.Purple);
});

scalar.WithApiReference(api);

#endregion

#region migrations

builder.AddProject<Projects.Aspire_Migrations>("aspire-migrations")
    .WithReference(dbStore).WaitFor(dbStore)
;

#endregion


#region aspire-api2

var api2 = builder.AddProject<Projects.Aspire_Api2>("aspire-api2")
    .WithReference(api);
scalar.WithApiReference(api2);

#endregion

#region gateway aspire integration

builder
    .AddYarp("yarp-oci")
    .WithConfiguration(yarp =>
    {
        var httpConfig = new HttpClientConfig
        {
            DangerousAcceptAnyServerCertificate = true
        };

        yarp.AddRoute(
            "/yarp-oci/{**catchall}",
            yarp.AddCluster(api).WithHttpClientConfig(httpConfig)
        ).WithTransformPathRemovePrefix("/yarp-oci");
    })
    .WaitFor(api)
    .WithUrlForEndpoint("http", url => url.Url = "/yarp-oci/weatherforecast");
#endregion


#region gateway aspire-yarp

builder.AddProject<Projects.Aspire_Yarp>("aspire-yarp")
    .WithUrlForEndpoint("https", url => url.Url = "/yarp/weatherforecast")
    .WithReference(api).WaitFor(api);
#endregion


#region gateway aspire-ocelot
builder.AddProject<Projects.Aspire_Ocelot>("aspire-ocelot")
    .WithUrlForEndpoint("https", url => url.Url = "/ocelot/weatherforecast")
    .WithReference(api).WaitFor(api);
#endregion



builder.Build().Run();