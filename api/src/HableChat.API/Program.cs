using HableChat.API.Extentions;
using HableChat.API.Setup;
using HableChat.Application;
using HableChat.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().ConfigureApiBehaviorOptions(x => {
    x.SuppressModelStateInvalidFilter  = true;
});
builder.Services.AddSwaggerGen();
builder.Services
    .AddApplication()
    .AddPersistence()
    .AddApiServices()
    .AddInterceptors()
    .AddAuthConfig();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseCors(cp => cp
    .AllowAnyOrigin()
    .SetPreflightMaxAge(TimeSpan.FromHours(24))
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseRouting();


app.AddAuth();
app.MapControllers();
app.Run();