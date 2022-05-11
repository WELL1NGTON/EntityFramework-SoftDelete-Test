using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SoftDelete.Test.Entities;

namespace SoftDelete.Test.Data.Configurations;

public abstract class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder
            .HasKey(lbda => lbda.Id);

        builder
            .Property(lbda => lbda.CreatedAt)
            .IsRequired();

        builder
            .Property(lbda => lbda.UpdatedAt)
            .IsRequired();
    }
}
