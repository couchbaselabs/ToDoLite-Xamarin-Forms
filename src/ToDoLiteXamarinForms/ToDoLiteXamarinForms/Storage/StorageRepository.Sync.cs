using System;
using System.Diagnostics;
using Couchbase.Lite;
using Couchbase.Lite.Auth;

namespace ToDoLiteXamarinForms.Storage
{
    public partial class StorageRepository
    {
        // TODO : Use demo Sync Gateway URL with default credentials.
        public readonly Uri RemoteSyncUrl = new Uri("http://cbdemo004bizz.cloudapp.net:4984/sync_gateway/");

        private Replication pull;
        private Replication push;
        private int lastPushCompleted;
        private int lastPullCompleted;
        private Replication _leader;

        public void InitializeSync()
        {
            if (Manager.SharedInstance.GetDatabase(DatabaseName) == null)
                return;

            UninitializeSync();

            IAuthenticator auth = 
                string.IsNullOrWhiteSpace(Username) 
                ? null
                : AuthenticatorFactory.CreateBasicAuthenticator(Username, Password);
            
            pull = Manager.SharedInstance.GetDatabase(DatabaseName).CreatePullReplication(RemoteSyncUrl);
            if(auth != null)
            {
                pull.Authenticator = auth;
            }
            
            push = Manager.SharedInstance.GetDatabase(DatabaseName).CreatePushReplication(RemoteSyncUrl);
            if(auth != null)
            {
                push.Authenticator = auth;
            }
            
            pull.Continuous = push.Continuous = true;
            
            pull.Changed += ReplicationProgress;
            push.Changed += ReplicationProgress;
            
            pull.Start();
            push.Start();
        }

        private void UninitializeSync()
        {
            if (pull != null)
            {
                pull.Changed -= ReplicationProgress;
                pull.Stop();
                pull = null;
            }

            if (push != null)
            {
                push.Changed -= ReplicationProgress;
                push.Stop();
                push = null;
            }
        }

        private void ReplicationProgress(object replication, ReplicationChangeEventArgs args)
        {
            if (args.Source.LastError != null)
            {
                var httpError = args.Source.LastError as HttpResponseException;
                if (httpError != null && httpError.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    LoginValid = false;
                    // TODO : Notify user about login failed
                    return;
                }
            }
            else
            {
                LoginValid = true;
            }

            var active = args.Source;
            Debug.WriteLine(String.Format("Push: {0}, Pull: {1}", push.Status, pull.Status));

            int lastTotal = 0;

            if (_leader == null)
            {
                if (active.IsPull && (pull.Status == ReplicationStatus.Active && push.Status != ReplicationStatus.Active))
                {
                    _leader = pull;
                }
                else if (!active.IsPull && (push.Status == ReplicationStatus.Active && pull.Status != ReplicationStatus.Active))
                {
                    _leader = push;
                }
                else
                {
                    _leader = null;
                }
            }
            if (active == pull)
            {
                lastTotal = lastPullCompleted;
            }
            else
            {
                lastTotal = lastPushCompleted;
            }

            Debug.WriteLine(
                String.Format(
                "Sync: {2} Progress: {0}/{1};",
                active.CompletedChangesCount - lastTotal,
                active.ChangesCount - lastTotal,
                active == push ? "Push" : "Pull"));

            var progress = (float)(active.CompletedChangesCount - lastTotal) / (float)(Math.Max(active.ChangesCount - lastTotal, 1));

            Debug.WriteLine(String.Format("({0:F})", progress));

            if (!(pull.Status != ReplicationStatus.Active && push.Status != ReplicationStatus.Active))
                if (progress < 1f)
                    return;
            if (active == null)
                return;

            lastPushCompleted = push.ChangesCount;
            lastPullCompleted = pull.ChangesCount;
        }
    }
}