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
                item.DocType = typeof(T).Name.ToLower();
                item.Id = item.DocType + "::" + Guid.NewGuid();
            }

            item.LastUpdated = DateTime.Now.Ticks.ToString();

            Document document = null;
            if (string.IsNullOrWhiteSpace(item.Id))
            {
                document = Manager.SharedInstance.GetDatabase(DatabaseName)
                    .CreateDocument();
            }
            else
            {
                document = Manager.SharedInstance.GetDatabase(DatabaseName)
                    .GetDocument(item.Id);
            }

            Dictionary<string, object> documentUserProperties = null;
            if (document.Properties != null && document.Properties.Any())
            {
                documentUserProperties = new Dictionary<string, object>(document.Properties);
            }
            else
            {
                documentUserProperties = new Dictionary<string, object>();
            }

            documentUserProperties["id"] = item.Id;
            documentUserProperties["lastupdated"] = item.LastUpdated;
            documentUserProperties["doctype"] = item.DocType;
            documentUserProperties["doc"] = item;

            throw new NotImplementedException("Save/put user properties document not implemented");
            // add code here ->

            return item;
        }

        public T Get<T>(string id) where T : ModelBase
        {
            var t = typeof(T).Name;
            string docId = id.Contains("::") ? id : typeof(T).Name.ToLower() + "::" + id;

            Document document = null;
            throw new NotImplementedException("Get document not implemented");
            // Add code here ->

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