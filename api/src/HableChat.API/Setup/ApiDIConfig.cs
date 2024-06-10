using System.Text;
using HableChat.API.Services;
using HableChat.Application.Common.Interfaces;
using HableChat.Domain.Users;
using HableChat.Infra.Data;
using HableChat.Infra.Data.HubConnections;
using HableChat.Infra.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HableChat.API.Setup;

public static class ApiDIConfig
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<IUserManager, UserManager>();
        services.AddCors(options =>
                options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
        services.AddSignalR();
        services.AddSingleton<SharedChannel>();

        return services;
    }

    public static IServiceCollection AddAuthConfig(this IServiceCollection services)
    {
        services.AddAuthentication(options => {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = "https://localhost:7266",
                ValidAudience = "https://localhost:7266",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a13e18ab8c8607457f7d8f8a818e731fac47a6099fbeb5e868440666bed9e76a"))
            };
        });
        services.AddAuthorization();
        services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<AppIdentityContext>();
        
        return services;
    }

    public static WebApplication AddAuth(this WebApplication app)
    {
        
        app.UseCors("CorsPolicy");
        app.MapIdentityApi<User>();
        app.MapHub<ChannelHub>("messages");
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}
