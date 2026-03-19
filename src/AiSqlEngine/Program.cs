using AiSqlEngine.Api;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Veracity.Common.Authentication;
using Veracity.Common.OAuth.Providers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "AiSqlEngine API", Version = "v1" });
});

builder.Services.AddApi();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddVeracity(builder.Configuration)
       .AddSingleton<IDistributedCache>(
           new MemoryDistributedCache(
               new OptionsWrapper<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions())))
       .Configure<CookiePolicyOptions>(options =>
       {
           options.CheckConsentNeeded = _ => true;
           options.MinimumSameSitePolicy = SameSiteMode.None;
           options.Secure = CookieSecurePolicy.Always;
       })
       .AddVeracityServices(builder.Configuration["Veracity:MyServicesApi"])
       .AddAuthentication(sharedOptions =>
       {
           sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
           sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
       })
       .AddVeracityAuthentication(builder.Configuration)
       .AddCookie(options =>
       {
           options.Cookie.SameSite = SameSiteMode.None;
           options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
       });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AiSqlEngine API v1");
});

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
