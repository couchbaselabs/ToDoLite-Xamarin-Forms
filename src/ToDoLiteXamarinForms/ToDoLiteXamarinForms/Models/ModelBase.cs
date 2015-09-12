
using System.Runtime.CompilerServices;

namespace ToDoLiteXamarinForms.Models
{
    public abstract class ModelBase
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string LastUpdated { get; set; }
    }
}