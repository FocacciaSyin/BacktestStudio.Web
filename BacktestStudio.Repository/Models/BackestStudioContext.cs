using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BacktestStudio.Repository.Models;

public partial class BackestStudioContext : DbContext
{
    public BackestStudioContext()
    {
    }

    public BackestStudioContext(DbContextOptions<BackestStudioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PurchaseRecord> PurchaseRecords { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PurchaseRecord>(entity =>
        {
            entity.HasIndex(e => e.Date, "idx_purchase_records_date");

            entity.HasIndex(e => e.Symbol, "idx_purchase_records_symbol");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("DATETIME");
            entity.Property(e => e.Date).HasColumnType("DATETIME");
            entity.Property(e => e.Price).HasColumnType("DECIMAL(10, 2)");
            entity.Property(e => e.Symbol)
                .HasDefaultValue("STOCK")
                .HasColumnType("VARCHAR(10)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("DATETIME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
