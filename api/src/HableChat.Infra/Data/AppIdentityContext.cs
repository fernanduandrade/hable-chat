using HableChat.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HableChat.Infra.Data;

public class AppIdentityContext : IdentityDbContext<User>
{
    public AppIdentityContext(DbContextOptions<AppIdentityContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("identity");
    }
}
