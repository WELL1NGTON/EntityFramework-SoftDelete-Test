using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SoftDelete.Test.Data.Extensions;
using SoftDelete.Test.Entities;

namespace SoftDelete.Test.Data.Configurations;

public sealed class ToDoEntryEntityTypeConfiguration : BaseEntityTypeConfiguration<ToDoEntryEntity>
{
    public override void Configure(EntityTypeBuilder<ToDoEntryEntity> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Title).IsRequired();

        builder.Property(e => e.Description).IsRequired(false);

        builder.Property(e => e.IsDone).IsRequired();

        builder.Property(e => e.DueDate).IsRequired();

        builder.ConfigureSoftDelete();
    }
}
