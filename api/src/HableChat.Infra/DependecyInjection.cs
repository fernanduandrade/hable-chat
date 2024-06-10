using HableChat.Application.Common.Interfaces;
using HableChat.Domain.Channels;
using HableChat.Domain.Guilds;
using HableChat.Domain.Messages;
using HableChat.Infra.Common;
using HableChat.Infra.Data;
using HableChat.Infra.Data.Repositories;
using HableChat.Infra.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace HableChat.Infra;

public static class DependecyInjection
{
    public static IServiceCollection AddInterceptors(this IServiceCollection services)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<PublishDomainEventsInterceptor>();
        return services;
    }
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<AppIdentityContext>(config => {
            config.UseNpgsql("Host=localhost;Port=5432;Database=hable_db;User Id=postgres;Password=postgres", options => {
                options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);
                options.MigrationsAssembly(typeof(AppIdentityContext).Assembly.FullName);
            });
        });

        services.AddDbContext<AppDbContext>(config => {
            config.UseNpgsql("Host=localhost;Port=5432;Database=hable_db;User Id=postgres;Password=postgres", options => {
                options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);
                options.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
            });
        });

        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

        services.AddScoped<IGuildRepository, GuildRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IChannelRepository, ChannelRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
