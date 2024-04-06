using Application.Configurations;
using WebAPI;
using WebAPI.Gateway.Configuration;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration.Get<AppConfiguration>();
if (configuration != null)
{
    configuration.DatabaseConnection = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
    configuration.Key = builder.Configuration["JwtSettings:Key"] ?? "";
    configuration.Issuer = builder.Configuration["JwtSettings:Issuer"] ?? "";
    configuration.Audience = builder.Configuration["JwtSettings:Audience"] ?? "";
    configuration.FireBaseApiKey = builder.Configuration["FireBaseSettings:ApiKey"] ?? "";
    configuration.FireBaseBucket = builder.Configuration["FireBaseSettings:Bucket"] ?? "";
    configuration.FireBaseAuthEmail = builder.Configuration["FireBaseSettings:AuthEmail"] ?? "";
    configuration.FireBaseAuthPassword = builder.Configuration["FireBaseSettings:AuthPassword"] ?? "";
    builder.Services.AddDependency(configuration.DatabaseConnection);
    builder.Services.AddApiService(configuration.Key, configuration.Issuer, configuration.Audience);
    builder.Services.AddSingleton(configuration);
}
//Gateway
builder.Services.Configure<MomoConfig>(builder.Configuration.GetSection(MomoConfig.ConfigName));
builder.Services.Configure<ZaloPayConfig>(builder.Configuration.GetSection(ZaloPayConfig.ConfigName));
builder.Services.Configure<DiscordConfig>(builder.Configuration.GetSection(DiscordConfig.ConfigName));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(WebAPI.DependencyInjection.LocalPolicy);

// app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();