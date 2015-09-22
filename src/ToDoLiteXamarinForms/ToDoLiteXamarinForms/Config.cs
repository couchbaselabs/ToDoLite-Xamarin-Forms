using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoLiteXamarinForms
{
    public class Config
    {
        private static Config _instance = null;
        public static Config Instance { get { if (_instance == null) _instance = new Config(); return _instance; } }

        public string SyncGatewayUri { get { return "http://try-cb.cloudapp.net:4984/sync_gateway/"; } }
    }
}
