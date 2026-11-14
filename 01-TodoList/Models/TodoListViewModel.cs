namespace TodoList.Models;

public class TodoListViewModel
{
    public List<TodoItem> Items { get; set; } = new();
    public TodoItem NewItem { get; set; } = new();
}
