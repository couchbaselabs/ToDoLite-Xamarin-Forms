using System.Diagnostics;
using System.Linq;
using ToDoLiteXamarinForms.Models;
using Couchbase.Lite;
using ToDoLiteXamarinForms.Helpers;
using System.Collections.ObjectModel;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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
                },"ver2");
			
			Manager.SharedInstance.GetDatabase(DatabaseName)
				.GetView("query-view")
				.SetMap((doc, emit) =>
				{
					var item = doc["doc"] as JContainer;
					
					if(item == null)
					{
						return;
					}

					var items =  item
						.Children()
						.Where(p => p is JProperty)
						.Cast<JProperty>()
						.Select(p => new { name = p.Name, value = p.Value.ToString().ToLower() });

						foreach (var prop in items)
						{
							var v = doc["type"].ToString() + "::" + prop.name;
							emit(prop.value, v);
						}
				}
				, "query1");

			Search ("a", 50, typeof(TodoList));

            var query = Manager.SharedInstance.GetDatabase(DatabaseName)
                .GetView("all-docs")
                .CreateQuery();

            query.Run()
            .ToList()
            .ForEach(row =>
            {
                ProcessDocument(row.Document);
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

		public List<object>  Search(string prefixWord, int limit, Type type)
		{
			List<object> list = new List<object> ();

			if (string.IsNullOrWhiteSpace (prefixWord)) {
				return list;
			}

			var query = Manager.SharedInstance.GetDatabase(DatabaseName)
				.GetView("query-view")
				.CreateQuery();

			query.IndexUpdateMode = IndexUpdateMode.Before;
			query.StartKey = prefixWord.ToLower();
			query.EndKey = query.StartKey.ToString() + '\uEFFF';
			query.Limit = limit;

			var queryResult = 
				query
				.Run ();

			var queryResultFiltered = queryResult;
			if (type != null) 
			{
				var filter = string.Format("{0}::", type.Name);
				queryResultFiltered
					.Where (item => item.Value.ToString ().StartsWith (filter));
			}

			foreach (var item in queryResultFiltered) {
				var doc = (item.Document.UserProperties["doc"]);// as JContainer).ToObject<type>();
				list.Add (doc);
			}

			return list;
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