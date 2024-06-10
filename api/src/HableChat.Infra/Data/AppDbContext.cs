using HableChat.Domain.Channels;
using HableChat.Domain.Guilds;
using HableChat.Domain.Messages;
using HableChat.Infra.Interceptors;
using MediatR;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace HableChat.Infra.Data;

public class AppDbContext : DbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public AppDbContext(DbContextOptions<AppDbContext> options,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor,
        PublishDomainEventsInterceptor publishDomainEventsInterceptor,
        IMediator mediator)
        : base(options) {
        _mediator= mediator;
        _auditableEntitySaveChangesInterceptor= auditableEntitySaveChangesInterceptor;
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    public DbSet<Channel> Channels => Set<Channel>();
    public DbSet<Guild> Guilds => Set<Guild>();
    public DbSet<Message> Messages => Set<Message>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Ignore<List<IDomainEvent>>().ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.HasDefaultSchema("public");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}
