using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using ToDoLiteXamarinForms.Models;
using Couchbase.Lite;

namespace ToDoLiteXamarinForms.Storage
{
    public partial class StorageRepository
    {
        public readonly string DatabaseName = "cblitesyncdb";

        public ObservableCollection<TodoList> TodoLists { get; set; }
        public Dictionary<string, ObservableCollection<TodoItem>> TodoItems { get; set; }

        public ObservableCollection<Document> ConflictItems { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public bool LoginValid { get; set; }

        public StorageRepository()
        {
            Debug.WriteLine("CONSTRUCTOR!");

            TodoLists = new ObservableCollection<TodoList>();
            TodoItems = new Dictionary<string, ObservableCollection<TodoItem>>();

            ConflictItems = new ObservableCollection<Document>();
            ConflictItems.CollectionChanged += ConflictItems_CollectionChanged;

            ResetDatabase();
            InitializeDatabase();
            InitializeQuery();

            //InitializeSync();
        }

        void ConflictItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (ConflictItems.Any())
            {
                // TODO: navigate to conflicts UI
            }
        }

        private void ResetDatabase()
        {
            Manager.SharedInstance.GetDatabase(DatabaseName).Delete();
        }

        private void InitializeDatabase()
        {
            Manager.SharedInstance.GetDatabase(DatabaseName)
                .Changed += (object sender, DatabaseChangeEventArgs e) =>
                {
                    var ids = e.Changes.Where(item => item.IsCurrentRevision).Select(item => item.DocumentId);

                    if(e.IsExternal)
                    {
                    }
                    
                    foreach(var id in ids)
                    {
                        ProcessDocument( 
                            Manager.SharedInstance.GetDatabase(DatabaseName).GetExistingDocument(id));
                    }
                };
        }
    }
}