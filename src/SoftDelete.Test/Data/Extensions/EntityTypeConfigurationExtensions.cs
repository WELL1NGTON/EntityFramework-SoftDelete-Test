using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SoftDelete.Test.Entities;

namespace SoftDelete.Test.Data.Extensions;

public static class EntityTypeConfigurationExtensions
{
    public static void ConfigureSoftDelete<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseEntity, ISoftDeletableEntity
    {
        builder
            .Property(lbda => lbda.DeletedAt)
            .IsRequired(false);

        builder
            .HasQueryFilter(lbda => lbda.DeletedAt == null);

        builder
            .Ignore(lbda => lbda.ForceDelete);
    }
}
