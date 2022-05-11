namespace SoftDelete.Test.Dtos;

#nullable disable
public class ToDoEntryDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DueDate { get; set; }

    public bool IsDone { get; set; }

    public Guid UserId { get; set; }

    public DateTime? DeletedAt { get; set; }
}
