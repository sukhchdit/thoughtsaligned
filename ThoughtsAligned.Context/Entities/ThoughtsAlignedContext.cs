using ThoughtsAligned.Models.Dto;
using ThoughtsAligned.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ThoughtsAligned.Context.Entities;

public partial class ThoughtsAlignedContext : DbContext
{
    public ThoughtsAlignedContext()
    {
    }

    public ThoughtsAlignedContext(DbContextOptions<ThoughtsAlignedContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<FileInformation> FileInformations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.ToTable("ErrorLog");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<FileInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FileInfo");

            entity.ToTable("FileInformation");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.FileName).HasMaxLength(250);
            entity.Property(e => e.FilePath).HasMaxLength(250);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.UpdateOn).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
