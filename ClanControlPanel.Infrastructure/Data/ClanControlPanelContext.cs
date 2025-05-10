using System;
using System.Collections.Generic;
using ClanControlPanel.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ClanControlPanel.Infrastructure.Data;

public partial class ClanControlPanelContext : DbContext
{
    public ClanControlPanelContext()
    {
    }

    public ClanControlPanelContext(DbContextOptions<ClanControlPanelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Schedule> Schedule { get; set; }
    public virtual DbSet<Player> Players { get; set; }
    public virtual DbSet<Item> Items { get; set; }
    public virtual DbSet<EventAttendance> EventAttendences { get; set; }
    public virtual DbSet<Event> Event { get; set; }
    public virtual DbSet<EventType> EventTypes { get; set; }
    public virtual DbSet<Equipment> Equipments { get; set; }
    public virtual DbSet<ClanMoney> ClanMoney { get; set; }
    public virtual DbSet<Squad> Squads { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ClanControlPanel;Username=postgres;Password=123");*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Player)
            .WithOne(p => p.User)
            .HasForeignKey<Player>(p => p.UserId);
        
        /*modelBuilder.Entity<ScheduleDb>()
            .HasOne(s => s.PlayerDb)
            .WithMany()
            .HasForeignKey(s => s.PlayerId);*/
        
        /*modelBuilder.Entity<GoldenDropAttendanceDb>()
            .HasOne(g => g.PlayerDb)
            .WithMany()
            .HasForeignKey(g => g.PlayerId);*/
        
        /*modelBuilder.Entity<ClanWarAttendanceDb>()
            .HasOne(c => c.PlayerDb)
            .WithMany()
            .HasForeignKey(c => c.PlayerId);*/
        
        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.Player)
            .WithMany(p => p.Equipments)
            .HasForeignKey(e => e.PlayerId);

        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.Item)
            .WithMany()
            .HasForeignKey(e => e.ItemId);
        
        /*modelBuilder.Entity<SquadDb>()
            .HasKey(s => s.Id)*/

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
