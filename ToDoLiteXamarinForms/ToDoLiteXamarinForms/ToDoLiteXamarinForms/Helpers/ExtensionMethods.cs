using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ToDoLiteXamarinForms.Models;
using Newtonsoft.Json;

namespace ToDoLiteXamarinForms.Helpers
{
    public static class ExtensionMethods
    {
        public static void Update<T>(this ObservableCollection<T> list, IEnumerable<object> items)
            where T : ModelBase
        {
            foreach (var item in items)
            {
                list.Update<T>(item);
            }
        }

        public static void Update<T>(this ObservableCollection<T> list, object obj)
            where T : ModelBase
        {
            T item = null;
            if (obj is T)
            {
                item = obj as T;
            }
            else
            {
                item = JsonConvert.DeserializeObject<T>(obj.ToString());
            }

            var old = list.SingleOrDefault(i => item.Id == i.Id);

            if (old != null)
            {
                list.Remove(old);
            }

            list.Insert(0,item);
        }
    }
}