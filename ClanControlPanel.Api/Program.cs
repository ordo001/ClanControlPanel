using ClanControlPanel.Application.Servises;
using ClanControlPanel.Application.Settings;
using ClanControlPanel.Core.Interfaces.Services;
using ClanControlPanel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClanControlPanel.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddScoped<IUserServices, UserServise>();
        builder.Services.AddScoped<IPlayerService, PlayerService>();
        builder.Services.AddScoped<ISquadService, SquadService>();
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
        builder.Services.AddScoped<IValidatorService, ValidatorService>();
        builder.Services.AddScoped <ClanControlPanelContext>();
        builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
        builder.Services.AddAuth(builder.Configuration);

        builder.Services.AddDbContext<ClanControlPanelContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}