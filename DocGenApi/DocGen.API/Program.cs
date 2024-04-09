using DocGen.API.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DocGen.Infrastructure.Data;
using Amazon.S3;
using Amazon.Runtime;
using DocGen.Core.Interfaces;
using DocGen.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

//AWS CONFIG
var awsOptions = builder.Configuration.GetSection("AWS");
var accessKey = awsOptions["AccessKey"];
var secretKey = awsOptions["SecretKey"];
var region = awsOptions["Region"];

if (!string.IsNullOrEmpty(accessKey) && !string.IsNullOrEmpty(secretKey) && !string.IsNullOrEmpty(region))
{
    var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
    var awsConfig = new AmazonS3Config { RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region) };
    builder.Services.AddSingleton<IAmazonS3>(new AmazonS3Client(awsCredentials, awsConfig));
}
else
{
    throw new InvalidOperationException("AWS credentials and region must be provided.");
}

var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = domain;
        options.Audience = builder.Configuration["Auth0:Audience"];
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, RegisteredUserHandler>();
builder.Services.AddScoped<IS3Service, S3Service>();
builder.Services.AddHttpClient<IOpenAIService, OpenAIService>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RegisteredUser", policy => policy.Requirements.Add(new RegisteredUserRequirement()
    ));
});

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();