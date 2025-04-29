using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ContactNotesAPI.Models;

public partial class ContactNotesDbContext : DbContext
{
    public ContactNotesDbContext()
    {
    }

    public ContactNotesDbContext(DbContextOptions<ContactNotesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=ContactNotesDb;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contacts__3214EC07E6A5099C");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Contacts__85FB4E389187773C").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Contacts__A9D10534964CA075").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notes__3214EC07CDBDA2E3");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Contact).WithMany(p => p.Notes)
                .HasForeignKey(d => d.ContactId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notes__ContactId__45F365D3");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC070EB4118B");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E42B6B6B5C").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
