using DatingApp.Application.Extensions;
using DatingApp.Common.Extensions;
using DatingApp.Infrastructure.Extensions;
using DatingApp.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddPresentationServices(builder.Configuration)
    .AddTelemetryExtensions("DatingApp");

var app = builder.Build();

await app.RunDatingApp();
