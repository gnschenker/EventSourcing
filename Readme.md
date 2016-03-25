# Introduction
This repository is accompanying the workshop about Event Sourcing. It contains all instructions needed to get going and also a working sample application.
The application is implemented in .NET and consists of a Web API who generates events that are then stored in the event store. It contains also a simple command line application that shows how a read model can be built from the events stored in the event store
We are using GetEventStore as the event store and Mongo DB to store the read model. The necessary binaries to run GetEventStore and Mongo DB are included in this repo.
# Event Sourcing
A client (e.g. Web UI) sends a request to the backend to create or modify some data.
The backend either accepts the request or it rejects it. If the request is accepted then data is modified.
Contrary to classical stateful applications we do not store the updated state in the database. Instead the backend generates an event which reflects what has changed and this event is persisted.
Over time as we apply various modification to one and the same object we get a stream of events, each one reflecting what has happened. Like this we are able to see the what, why and when in detail. In a stateful application we cannot do this since with each modification we overwrite the previous state.

Of course storing streams of events in the event store doesn't make it very accessible for queries. For this we need to generate a read model. This read model is modeled in a way that it best suits the needs of the UI.

#The Sample Application
##GetEventStore
Download GetEventStore from [here](http://geteventstore.com). GetEventStore is secured and you need a username and password
By default it is admin/changeit. The default TCP port on which GES is listening is 1113 and the HTTP port 2113. 

To run GES open a command prompt and navigate to the folder where you installed it. Then you can use a command similar to this
```
EventStore.ClusterNode.exe --db [your-data-folder]
```
where `[your-data-folder]` is any folder where you want GES to store its data.
Open a browser and navigate to http://localhost:2113/ to admin GES. You need to enter you credentials admin/changeit to access the dashboard.
I have added a batch file `RunEventStore.cmd` to the repository which you can use instead.
