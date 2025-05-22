using LicenseDelivery.API.LicenseDelivery.GetLicense;
using LicenseDelivery.API.LicenseDelivery.Infrastructure.GoogleDrive;
using Microsoft.Extensions.Options;
using static Google.Apis.Drive.v3.DriveService;

var builder = WebApplication.CreateBuilder(args);

// Add sevices to the container

// Application Services
builder.Services.AddCarter(null, config =>
{
    var modules = typeof(Program).Assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();
    config.WithModules(modules);
});

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    //config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    //config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);



builder.Services.AddSingleton(sp =>
{
    DriveService service;
    GoogleCredential credential;
    var path = builder.Configuration["GoogleDrive:CredentialPath"]!;
    using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
    {
        credential = GoogleCredential.FromStream(stream);
    }
    if (credential.IsCreateScopedRequired)
    {
        credential = credential.CreateScoped(
            new[]
            {
                ScopeConstants.DriveFile,
                ScopeConstants.DriveReadonly
            });
    }
    service = new DriveService(
           new BaseClientService.Initializer()
           {
               HttpClientInitializer = credential,
               ApplicationName = builder.Configuration["GoogleDrive:ApplicationName"]!
           });
    return service;
}).AddTransient<GoogleDriveClient>();

builder.Services.AddSingleton<GoogleDriveClient>();
builder.Services
    .Configure<GoogleDriveOptions>(
         builder.Configuration.GetSection("GoogleDrive")
    )
    .AddSingleton(sp =>
         sp.GetRequiredService<IOptions<GoogleDriveOptions>>().Value
    );


var app = builder.Build();

// Configure the http request pipeline

app.MapCarter();

app.UseExceptionHandler(opts => { });

app.Run();
