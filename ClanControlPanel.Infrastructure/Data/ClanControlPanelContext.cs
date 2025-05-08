using System;
using System.Collections.Generic;
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

    public virtual DbSet<UserDb> Users { get; set; }
    public virtual DbSet<ScheduleDb> Schedule { get; set; }
    public virtual DbSet<PlayerDb> Players { get; set; }
    public virtual DbSet<ItemDb> Items { get; set; }
    public virtual DbSet<EventAttendenceDb> EventAttendences { get; set; }
    public virtual DbSet<EventDb> Event { get; set; }
    public virtual DbSet<EventTypeDb> EventTypes { get; set; }
    public virtual DbSet<EquipmentDb> Equipments { get; set; }
    public virtual DbSet<ClanMoneyDb> ClanMoney { get; set; }
    public virtual DbSet<SquadDb> Squads { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ClanControlPanel;Username=postgres;Password=123");*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserDb>()
            .HasOne(u => u.Player)
            .WithOne(p => p.User)
            .HasForeignKey<PlayerDb>(p => p.UserId);
        
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
        
        modelBuilder.Entity<EquipmentDb>()
            .HasOne(e => e.Player)
            .WithMany(p => p.Equipments)
            .HasForeignKey(e => e.PlayerId);

        modelBuilder.Entity<EquipmentDb>()
            .HasOne(e => e.Item)
            .WithMany()
            .HasForeignKey(e => e.ItemId);
        
        /*modelBuilder.Entity<SquadDb>()
            .HasKey(s => s.Id)*/

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
