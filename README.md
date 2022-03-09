⚠️ This repo is obsolete.  It is based on a version of Couchbase Lite that reached end of life years ago.

## ToDoLite-Xamarin-Forms
This is a demo app showing how to use the [Couchbase Lite][CBL] framework to embed a nonrelational ("NoSQL") document-oriented database in an Xamarin Forms app and sync it with [Couchbase Server][CBS] in "the cloud".


### Benefits of Couchbase Lite

What does Couchbase Lite bring to the table?

* **Transparent data sync.** By now, users practically _expect_ that data they enter on one device will be accessible from others, including their laptops. Couchbase Lite makes this easy. The app code operates on the local database, and bidirectional sync happens in the background.
* **Flexible, schemaless database.** Documents are stored as JSON, though they can be accessed as native Objective-C objects for convenience. There is no predefined schema. If you want to add new features like due dates or photo attachments, you won't have to deal with data migrations. The data will even interoperate with older versions of the app.
* **Multi-user capability.** With the Couchbase Sync Gateway, any number of users can securely sync with a single server database and share only the data they want to. The design of the gateway makes writing collaborative and social apps extremely easy.
* **Control over the back-end server.** You're not dependent on a big company to host everyone's data for you: you can run your own server, whether in a data center, on a host like EC2, or just on a spare PC in your office. It's even possible (though this app doesn't show how) to synchronize directly between two devices (P2P), with no server.
* **Cross-platform.** Couchbase Lite currently supports iOS, Android and Mac OS X, and its underlying data formats and protocols (as well as source code) are fully open.


## Building & Running The Demo App

Down to business: You'll need a Mac with Xamarin installed to compile and run this app.

1. Clone or download this repository.
2. Open in Visual Studio.
3. Restore all Nuget Packages. (will be done by Visual Studio on first build)
4. In Solution Manager, select Android or Ios as your start-up project.
6. Click the Run button

That's it! 

## Quick modifications you might want to make.
Change the Sync Url to point to your personal Sync Gateway Server.

If you run your own sync gateway, the sync function source code we use is available in the `sync-gateway-config.json` file in the root of this repository.

## To add the framework to your existing Xcode project

Please see the documentation for [Couchbase Lite][CBL].


## License

Released under the Apache license, 2.0.

Copyright 2011-2015, Couchbase, Inc.


[CBL]: https://github.com/couchbase/Couchbase-Lite-iOS/
[CBS]: http://www.couchbase.com/couchbase-server/overview
[TODO_PHONEGAP]: https://github.com/couchbaselabs/TodoLite-PhoneGap
[LIST]: https://groups.google.com/group/mobile-couchbase
[CBL_DOWNLOAD]: http://www.couchbase.com/download#cb-mobile
[CBL_BUILD]: https://github.com/couchbase/couchbase-lite-ios/wiki/Building-Couchbase-Lite
