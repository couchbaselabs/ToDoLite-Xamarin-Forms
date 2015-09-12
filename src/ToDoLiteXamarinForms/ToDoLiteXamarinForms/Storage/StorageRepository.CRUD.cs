using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ToDoLiteXamarinForms.Models;
using Couchbase.Lite;
using Newtonsoft.Json;

namespace ToDoLiteXamarinForms.Storage
{
    public partial class StorageRepository
    {
        public T Upsert<T>(T item) where T : ModelBase
        {
            if (string.IsNullOrWhiteSpace(item.Id))
            {
                item.Type = typeof(T).Name.ToLower();
                item.Id = item.Type + "::" + Guid.NewGuid();
            }

            item.LastUpdated = DateTime.Now.Ticks.ToString();

            Document document =
                !string.IsNullOrWhiteSpace(item.Id) ?
                document = Manager.SharedInstance.GetDatabase(DatabaseName)
                    .GetDocument(item.Id) :
                document = Manager.SharedInstance.GetDatabase(DatabaseName)
                    .CreateDocument();

            var updatedProperties =
                document.Properties != null && document.Properties.Any() ?
                new Dictionary<string, object>(document.Properties) :
                new Dictionary<string, object>();

            updatedProperties["id"] = item.Id;
            updatedProperties["lastupdated"] = item.LastUpdated;
            updatedProperties["doctype"] = item.Type;
            updatedProperties["doc"] = item;

            document.PutProperties(updatedProperties);

            return item;
        }

        public T Get<T>(string id) where T : ModelBase
        {
            var t = typeof(T).Name;
            string docId = id.Contains("::") ? id : typeof(T).Name.ToLower() + "::" + id;

            Document document = Manager.SharedInstance
                .GetDatabase(DatabaseName)
                .GetDocument(docId);

            if (document == null)
            {
                return default(T);
            }

            if (document.UserProperties == null)
            {
                return default(T);
            }

            if (!document.UserProperties.ContainsKey("doc"))
            {
                return default(T);
            }

            T model = null;

            try
            {
                var obj = document.UserProperties["doc"];

                if (obj is T)
                {
                    return obj as T;
                }
                else
                {
                    model = JsonConvert.DeserializeObject<T>(obj.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return model;
        }
    }
}