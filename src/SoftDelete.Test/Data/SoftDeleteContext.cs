using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using SoftDelete.Test.Entities;

namespace SoftDelete.Test.Data;

public class SoftDeleteContext : DbContext
{
    public SoftDeleteContext(DbContextOptions options) : base(options)
    {
        Users = Set<UserEntity>();
        ToDoEntries = Set<ToDoEntryEntity>();
    }

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<ToDoEntryEntity> ToDoEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.EnableSensitiveDataLogging();

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SoftDeleteEntries(ChangeTracker.Entries());
        SetTimeTracking(ChangeTracker.Entries());

        return base.SaveChangesAsync(cancellationToken);
    }

    private static void SetTimeTracking(IEnumerable<EntityEntry> entries)
    {
        var udpatedEntries = entries
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified && e.Entity is BaseEntity);

        foreach (var entry in udpatedEntries)
        {
            var entity = (BaseEntity)entry.Entity;
            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

            entity.UpdatedAt = DateTime.UtcNow;
        }
    }

    private static void SoftDeleteEntries(IEnumerable<EntityEntry> entries)
    {
        var deletedEntries = entries
            .Where(e => e.State == EntityState.Deleted && e.Entity is ISoftDeletableEntity);

        foreach (var entry in deletedEntries.Where(e => ((ISoftDeletableEntity)e.Entity).ForceDelete == false))
        {
            entry.State = EntityState.Unchanged;
            var entity = (ISoftDeletableEntity)entry.Entity;
            entity.DeletedAt = DateTime.UtcNow;
        }
    }
}
