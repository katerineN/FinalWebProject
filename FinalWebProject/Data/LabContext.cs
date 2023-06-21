using System.Drawing;
using FinalWebProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalWebProject.Data;

public class LabContext : DbContext
{
    public LabContext(DbContextOptions<LabContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<PointS> Points { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=LabDatabase.db");
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("student", "lab");
            entity.HasKey(e => e.Id).HasName("pk_student");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.last_name)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.first_name)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.course).HasColumnName("course");
            entity.Property(e => e.group).HasColumnName("group");

        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("subject", "lab");
            entity.HasKey(e => e.Id).HasName("pk_subject");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.teacher_name)
                .HasMaxLength(100)
                .HasColumnName("teacher_name");
        });

        modelBuilder.Entity<PointS>(entity =>
        {
            entity.ToTable("point", "lab");
            entity.HasKey(e => e.Id).HasName("pk_point");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.value).HasColumnName("value");
            entity.Property(e => e.subject_id).HasColumnName("subject_id");
            entity.Property(e => e.student_id).HasColumnName("student_id");

            entity.HasOne(d => d.Subject).WithMany(p => p.Points)
                .HasForeignKey(d => d.subject_id)
                .HasConstraintName("fk_subject");

            entity.HasOne(d => d.Student).WithMany(p => p.Points)
                .HasForeignKey(d => d.student_id)
                .HasConstraintName("fk_student");
        });
    }
}