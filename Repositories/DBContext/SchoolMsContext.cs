using System;
using System.Collections.Generic;
using DomainEntities.DBEntities;
using Microsoft.EntityFrameworkCore;

namespace DomainEntities
{
    public partial class SchoolMsContext : DbContext
    {
        public SchoolMsContext() { }

        public SchoolMsContext(DbContextOptions<SchoolMsContext> options)
            : base(options) { }

        public virtual DbSet<Governorate> Governorates { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<StudentSubject> StudentSubjects { get; set; }
        public virtual DbSet<Mark> Marks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=MOHAMMAD_JUNIER;Database=SchoolMS;Trusted_Connection=True;User=sa;Password=M0hamm@d28;Integrated Security=False;TrustServerCertificate=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Governorate
            modelBuilder.Entity<Governorate>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            // Student
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.BirthDate).HasColumnType("datetime");
                entity.Property(e => e.CreateDate).HasColumnType("datetime");
                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.GovernorateObj)
                      .WithMany(p => p.Students)
                      .HasForeignKey(d => d.Governorate)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Students_Governorates");
            });

            // Subject
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            // StudentSubjects (enrollments)
            modelBuilder.Entity<StudentSubject>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.SubjectId });

                entity.HasOne(e => e.Student)
                      .WithMany(s => s.StudentSubjects)
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.Subject)
                      .WithMany(s => s.StudentSubjects)
                      .HasForeignKey(e => e.SubjectId)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.ToTable("StudentSubjects");
            });

            // Marks (grades)
            modelBuilder.Entity<Mark>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.SubjectId });

                entity.Property(e => e.Mark1)
                      .HasColumnType("decimal(5, 2)")
                      .HasColumnName("Mark");

                entity.HasOne(d => d.Student)
                      .WithMany(p => p.Marks)
                      .HasForeignKey(d => d.StudentId)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Subject)
                      .WithMany(p => p.Marks)
                      .HasForeignKey(d => d.SubjectId)
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.ToTable("Marks");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
