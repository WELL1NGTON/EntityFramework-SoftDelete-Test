namespace SoftDelete.Test.Entities;

public interface ISoftDeletableEntity
{
    DateTime? DeletedAt { get; set; }

    bool ForceDelete { get; set; }
}
