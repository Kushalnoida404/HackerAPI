using Hacker.API.Infrastructure;
using Hacker.Application.News;
using Hacker.Core.Core;
using Hacker.Core.News;
using HackerAPI.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", policy =>
{
    policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var apiClientSettings = config.GetSection("ApiClient");
builder.Services.AddHttpClient();
builder.Services.AddTransient(typeof(IApiClient<>), typeof(ApiClient<>));

builder.Services.AddSingleton(apiClientSettings.GetSection("HackerAPI").Get<ApiSettings<HackerApiEndpoints>>())
    .AddSingleton<INewsService, NewsService>()
    .AddSingleton<INewsRepository, NewsRepository>();

builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandler>();
app.UseAuthorization();

app.MapControllers();

app.Run();
