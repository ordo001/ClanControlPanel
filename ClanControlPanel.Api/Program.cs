using ClanControlPanel.Api.Middleware;
using ClanControlPanel.Application.Servises;
using ClanControlPanel.Application.Settings;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Infrastructure.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

namespace ClanControlPanel.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.ListenAnyIP(5220); // <-- ВАЖНО
        });
        builder.WebHost.UseUrls("http://0.0.0.0:5220");
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddScoped<IUserServices, UserServise>();
        builder.Services.AddScoped<IPlayerService, PlayerService>();
        builder.Services.AddScoped<ISquadService, SquadService>();
        builder.Services.AddScoped<IEventService, EventService>();
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
        builder.Services.AddScoped<IValidatorService, ValidatorService>();
        builder.Services.AddScoped <ClanControlPanelContext>();
        builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
        builder.Services.AddAuth(builder.Configuration);

        builder.Services.AddDbContext<ClanControlPanelContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        var app = builder.Build();
        
        app.UseMiddleware<ExceptionMiddleware>();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        /*app.UseHttpsRedirection();*/
        app.UseForwardedHeaders();
        
        app.UseAuthorization();

        app.UseCors(x =>
        {
            x.WithHeaders().AllowAnyHeader();
            x.WithOrigins("https://bi-casual-determines-perform.trycloudflare.com");
            /*x.WithOrigins("http://localhost:5173");*/
            x.WithMethods().AllowAnyMethod();
            x.AllowCredentials();
        });
        

        app.MapControllers();

        app.Run();
    }
}