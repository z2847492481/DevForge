using DevForge.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevForge.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Requirement> Requirements => Set<Requirement>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<RequirementApplication> RequirementApplications => Set<RequirementApplication>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---- users ----------------------------------------------------------
        var user = modelBuilder.Entity<User>();
        user.ToTable("users");
        user.HasKey(u => u.Id);
        user.Property(u => u.Id).HasColumnName("id").UseIdentityColumn();
        user.Property(u => u.Username).HasColumnName("username").HasMaxLength(255).IsRequired();
        user.Property(u => u.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
        user.Property(u => u.PasswordHash).HasColumnName("password_hash").IsRequired();
        user.Property(u => u.FullName).HasColumnName("full_name").HasMaxLength(255);
        user.Property(u => u.Role).HasColumnName("role").HasMaxLength(50).IsRequired();
        user.Property(u => u.IsActive).HasColumnName("is_active");
        user.Property(u => u.CreatedAt).HasColumnName("created_at");
        user.Property(u => u.UpdatedAt).HasColumnName("updated_at");
        user.HasIndex(u => u.Username).IsUnique().HasDatabaseName("uq_users_username");
        user.HasIndex(u => u.Email).IsUnique().HasDatabaseName("uq_users_email");

        // ---- requirements ---------------------------------------------------
        var requirement = modelBuilder.Entity<Requirement>();
        requirement.ToTable("requirements");
        requirement.HasKey(r => r.Id);
        requirement.Property(r => r.Id).HasColumnName("id").UseIdentityColumn();
        requirement.Property(r => r.Title).HasColumnName("title").HasMaxLength(500).IsRequired();
        requirement.Property(r => r.Description).HasColumnName("description");
        requirement.Property(r => r.Status).HasColumnName("status").HasMaxLength(50).IsRequired();
        requirement.Property(r => r.Priority).HasColumnName("priority").HasMaxLength(50).IsRequired();
        requirement.Property(r => r.CreatedBy).HasColumnName("created_by");
        requirement.Property(r => r.CreatedAt).HasColumnName("created_at");
        requirement.Property(r => r.UpdatedAt).HasColumnName("updated_at");
        requirement.HasOne(r => r.Creator)
            .WithMany()
            .HasForeignKey(r => r.CreatedBy)
            .HasConstraintName("fk_requirements_created_by");
        requirement.HasIndex(r => r.CreatedBy).HasDatabaseName("idx_requirements_created_by");
        requirement.HasIndex(r => r.Status).HasDatabaseName("idx_requirements_status");

        // ---- applications ---------------------------------------------------
        var application = modelBuilder.Entity<Application>();
        application.ToTable("applications");
        application.HasKey(a => a.Id);
        application.Property(a => a.Id).HasColumnName("id").UseIdentityColumn();
        application.Property(a => a.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
        application.Property(a => a.Description).HasColumnName("description");
        application.Property(a => a.AppType).HasColumnName("app_type").HasMaxLength(50);
        application.Property(a => a.Status).HasColumnName("status").HasMaxLength(50).IsRequired();
        application.Property(a => a.OwnerId).HasColumnName("owner_id");
        application.Property(a => a.CreatedAt).HasColumnName("created_at");
        application.Property(a => a.UpdatedAt).HasColumnName("updated_at");
        application.HasOne(a => a.Owner)
            .WithMany()
            .HasForeignKey(a => a.OwnerId)
            .HasConstraintName("fk_applications_owner");
        application.HasIndex(a => a.OwnerId).HasDatabaseName("idx_applications_owner_id");
        application.HasIndex(a => a.Name).IsUnique().HasDatabaseName("uq_applications_name");

        // ---- requirement_applications ---------------------------------------
        var link = modelBuilder.Entity<RequirementApplication>();
        link.ToTable("requirement_applications");
        link.HasKey(ra => new { ra.RequirementId, ra.ApplicationId, ra.TraceType });
        link.Property(ra => ra.RequirementId).HasColumnName("requirement_id");
        link.Property(ra => ra.ApplicationId).HasColumnName("application_id");
        link.Property(ra => ra.TraceType).HasColumnName("trace_type").HasMaxLength(50).IsRequired();
        link.Property(ra => ra.CreatedAt).HasColumnName("created_at");
        link.HasOne(ra => ra.Requirement)
            .WithMany()
            .HasForeignKey(ra => ra.RequirementId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_req_app_requirement");
        link.HasOne(ra => ra.Application)
            .WithMany()
            .HasForeignKey(ra => ra.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_req_app_application");
        link.HasIndex(ra => ra.ApplicationId).HasDatabaseName("idx_req_app_application");
    }
}