using System.Diagnostics;
using System.Linq;
using ToDoLiteXamarinForms.Models;
using Couchbase.Lite;
using ToDoLiteXamarinForms.Helpers;
using System.Collections.ObjectModel;
using System;

namespace ToDoLiteXamarinForms.Storage
{
    public partial class StorageRepository
    {
        private void InitializeQuery()
        {
            Manager.SharedInstance.GetDatabase(DatabaseName)
                .GetView("all-docs")
                .SetMap((doc, emit) =>
                {
                    emit(doc["_id"], null);
                },
                "ver2");

            var query = Manager.SharedInstance.GetDatabase(DatabaseName)
                .GetView("all-docs")
                .CreateQuery();

            query.Run()
            .ToList()
            .ForEach(row =>
            {
                throw new NotImplementedException("Call ProcessDocument with the document in the row");
                // add core here->
            });

            var liveQuery = Manager.SharedInstance.GetDatabase(DatabaseName)
                .GetExistingView("all-docs")
                .CreateQuery()
                .ToLiveQuery();

            liveQuery.Changed += (object sender, QueryChangeEventArgs e) =>
            {

                var conflicts = e.Rows
                    .Where(item => item.Document.ConflictingRevisions.Any())
                    .Select(item => item.Document)
                    ;

                foreach (var item in conflicts)
                {
                    ConflictItems.Add(item);
                }

                Debug.WriteLine("liveQuery.Changed " + e.Rows.Count);
            };

            liveQuery.Start();
        }

        private void ProcessDocument(Document doc)
        {
            if (doc.Id.Contains("::"))
            {
                var doctype = doc.CurrentRevision.UserProperties["doctype"].ToString();

                if (doctype == typeof(TodoList).Name.ToLower())
                {
                    if (!TodoItems.ContainsKey(doc.Id))
                    {
                        TodoItems.Add(doc.Id, new ObservableCollection<TodoItem>());
                    }

                    TodoLists.Update<TodoList>(doc.UserProperties["doc"]);
                }
                else if (doctype == typeof(TodoItem).Name.ToLower())
                {
                    var todo = Get<TodoItem>(doc.Id);

                    if (!TodoItems.ContainsKey(todo.TodoListId))
                    {
                        TodoItems.Add(todo.TodoListId, new ObservableCollection<TodoItem>());
                    }

                    TodoItems[todo.TodoListId].Update<TodoItem>(todo);
                }
            }
        }
    }
}