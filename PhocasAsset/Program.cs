var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
//Register Services
builder.Services.Configure<AwsDynamoOptions>(builder.Configuration.GetSection(AwsDynamoOptions.AwsDynamo));
builder.Services.AddSingleton<IAssetService, AssetService>();
builder.Services.AddSingleton<IDataAccess, DataAccess>();
builder.Services.AddHostedService<DbInitializerService>();

builder.AddSwaggerDocumentation();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseSwaggerUI();

app.MapEndpoints();

app.Run();
