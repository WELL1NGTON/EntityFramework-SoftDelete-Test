namespace SoftDelete.Test.Entities;

public sealed class ToDoEntryEntity : BaseEntity, ISoftDeletableEntity
{
    public ToDoEntryEntity(string title, string? description, DateTime dueDate, UserEntity user) : this(title, description, dueDate, user.Id)
    {
        User = user;
    }

    public ToDoEntryEntity(string title, string? description, DateTime dueDate, Guid userId)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        UserId = userId;
    }


    public string Title { get; set; }

    public string? Description { get; set; }

    public DateTime DueDate { get; set; }

    public bool IsDone { get; set; }

    public UserEntity? User { get; private set; }

    public Guid UserId { get; private set; }

    public DateTime? DeletedAt { get; set; }

    public bool ForceDelete { get; set; }
}
