using System.Collections.ObjectModel;
using ToDoLiteXamarinForms.Models;

namespace ToDoLiteXamarinForms.ViewModels
{
    public class TodoDetailsViewModel : ViewModelBase
    {
        private object item;

        public object Item
        {
            get
            {
                return item;
            }
            set
            {
                if (item != value)
                {
                    item = value;
                    OnPropertyChanged("Item");
                }
            }
        }

        public ObservableCollection<TodoItem> Items { get; set; }

    }
}
