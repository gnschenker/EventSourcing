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
This sample application consists of a ASP.NET Web API that has a few endpoints defined to which a client can POST commands (or in web terminology also called *requests*). The logic behind the API either accepts those commands or not. If it does then the commands are converted into events. Those events are then stored in an Event Store type database. We use GetEventStore for this purpose. There is a nuget package with a client for GetEventStore available that we use in this sample to acces the DB.
Since an event store is not really suited for querying data we have also implemented a simple command line tool that registers itself with the event store. The event store will then forward all events that are stored in its DB asynchronously to this tool. The tool will deserialize the events and dispatch them to *observers*. Each observer is responsible to create a view from the events it gets. In our case a view will be a Mongo DB collection.
##API##
As said above, we use ASP.Net Web API to implement our (public) API. So far we have defined the following endpoints

To add a new project
```
[POST] 
URI:     localhost:[port]/api/projects
Header:  Content-Type: application/json
Body:    {"name": "some project name"}
```
To assing a PM to an existing project
```
[POST] 
URI:     localhost:[port]/api/projects/pm
Header:  Content-Type: application/json
Body:    {
	        "projectId": "[id of the project (GUID)]",
			"staffId": "[id of the PM (GUID)]"
		 }
```
##Read model generator##
The sample contains a .NET console application which is responsible to build the read model. It uses Mongo DB as its target database. To access Mongo DB from C# we use the official Mongo DB client nuget package. The tool implements one sample *observer* that takes events and converts them into a read model. The observer is called `ProjectsObserver`. The tool registers itself with GetEventStore and asks for all events that have so far been written to the event store (this is called a catch up subscription). GetEventStore will thus first send all events that it already has to the tool and after that all new incoming events. This is an asynchronous process.
##GetEventStore
GetEventStore (GES) is part of the repository or you can download GES from [here](http://geteventstore.com). GES is secured and you need a username and password
By default it is admin/changeit. The default TCP port on which GES is listening is 1113 and the HTTP port 2113. 

To run GES open a command prompt and navigate to the folder where you installed it. Then you can use a command similar to this
```
EventStore.ClusterNode.exe --db [your-data-folder]
```
where `[your-data-folder]` is any folder where you want GES to store its data.
Open a browser and navigate to http://localhost:2113/web to admin GES. You need to enter you credentials admin/changeit to access the dashboard.
I have added a batch file `RunEventStore.cmd` to the repository which you can use instead.
##Mongo DB
In this sample we are using Mongo DB to store our read model. But any other relational or non-relational DB can be used to store the read model. You decide what DB suits your needs best.
The Mongo DB binaries are also part of this repository. To run Mongo DB we need to at least define a logging and a data directory. This is done in the Powershell script `RunMongoDb.ps1`. When executing the script a config file for Mongo DB is created and the DB is started as a background service.
To work with Mongo DB we recommend *Mongo Vue*. This tool is free to use and can be downloaded form [here](http://www.mongovue.com/).