
namespace ToDoLiteXamarinForms.Models
{
    public class TodoItem : ModelBase
    {
        public bool Done { get; set; }
        
        public string Details { get; set; }
        
        public string TodoListId { get; set; }
    }
}
