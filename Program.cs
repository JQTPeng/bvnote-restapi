using System.Text.Json.Serialization;
using Scalar.AspNetCore;
using bvnote_restapi.Data;
using bvnote_restapi.Model;
using bvnote_restapi.RouteGroup;
using bvnote_restapi.Servics;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR().AddJsonProtocol(opts => { opts.PayloadSerializerOptions.PropertyNamingPolicy = null; });
builder.Services.AddScoped<IBibleDb>(sp => new BibleDb(config.GetConnectionString("BibleDb"), sp.GetRequiredService<ILogger<BibleDb>>()));
builder.Services.AddOpenApi();
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder =>
        builder.Expire(TimeSpan.FromSeconds(15)));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("origin_BlazorChatExample", policy =>
    {
        var blazor_chat_origins = new[]
        {
            "https://webcammeeting-fc489.web.app/",
            "https://webcammeeting-fc489.firebaseapp.com/",
            "http://localhost:5206"
        };

        policy.WithOrigins(blazor_chat_origins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

// TODO: app.UseHttpsRedirection();
app.UseOutputCache();
// TODO: app.UseAuthentication()
// TODO: app.UseAuthorization()
app.UseRouting();
app.UseCors();

app.MapGroup("/api/v1")
    .MapBible();

app.MapHub<ChatHub>("/api/v1/chatHub")
    .RequireCors("origin_BlazorChatExample");

app.MapOpenApi();
app.MapScalarApiReference();
app.Run();


[JsonSerializable(typeof(UserConnection))]
[JsonSerializable(typeof(List<Book>))]
[JsonSerializable(typeof(List<Verse>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}