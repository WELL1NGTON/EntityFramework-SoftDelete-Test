using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SoftDelete.Test.Entities;

namespace SoftDelete.Test.Data.Configurations;

public sealed class UserEntityTypeConfiguration : BaseEntityTypeConfiguration<UserEntity>
{
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        base.Configure(builder);

        builder
            .Property(lbda => lbda.Name)
            .IsRequired();
    }
}
