using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClanControlPanel.Infrastructure.Data;

public partial class ControlPanelContext : DbContext
{
    public ControlPanelContext()
    {
    }

    public ControlPanelContext(DbContextOptions<ControlPanelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserDb> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ControlPanel;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserDb>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("Users_pkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
