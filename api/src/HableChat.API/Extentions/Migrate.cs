using HableChat.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace HableChat.API.Extentions;

public static class Migrate
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        var provider = app.ApplicationServices.CreateScope();
        var identityContexnt = provider.ServiceProvider.GetRequiredService<AppDbContext>(); 
        var appContext = provider.ServiceProvider.GetRequiredService<AppDbContext>();
        
        identityContexnt.Database.Migrate();
        appContext.Database.Migrate();
    }
}
