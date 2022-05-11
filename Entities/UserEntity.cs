namespace SoftDelete.Test.Entities;

public sealed class UserEntity : BaseEntity
{
    private readonly List<ToDoEntryEntity> _toDoEntries = new();

    public UserEntity(string name) : base()
    {
        Name = name;
    }

    public string Name { get; set; }

    public IReadOnlyCollection<ToDoEntryEntity> ToDoEntries => _toDoEntries.AsReadOnly();

    public void AddToDoEntry(ToDoEntryEntity toDoEntry)
    {
        _toDoEntries.Add(toDoEntry);
    }

    public void RemoveToDoEntry(ToDoEntryEntity toDoEntry)
    {
        _toDoEntries.Remove(toDoEntry);
    }
}
