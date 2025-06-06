﻿using System;
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
    public virtual DbSet<EventAttendance> EventAttendances { get; set; }
    public virtual DbSet<Event> Events { get; set; }
    public virtual DbSet<EventType> EventTypes { get; set; }
    public virtual DbSet<EventStage> EventStages { get; set; }
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

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();
        
        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.Player)
            .WithMany(p => p.Equipments)
            .HasForeignKey(e => e.PlayerId);

        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.Item)
            .WithMany()
            .HasForeignKey(e => e.ItemId);
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
