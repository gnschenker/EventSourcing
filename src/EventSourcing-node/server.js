var express = require('express');
var app = express();
var uuid = require('node-uuid');
var eventstore = require('geteventstore-promise');
var client = eventstore.http({
                hostname: 'localhost',
                port: 2113,
                credentials: {
                    username: 'admin',
                    password: 'changeit'
                }
            });

var bodyParser = require('body-parser');
app.use(bodyParser());

app.post('/projects', function(req, res){
  var id = uuid.v4();
  var streamName = 'project-' + id;
  client.writeEvent(streamName, 'projectAdded', { id: id, name: req.body.name})
    .then(function(){
      console.log('Project added event written')
      res.send(id, 201);
    });
});

app.post('/projects/:id/pm', function(req, res){
  var id = req.params.id;
  var pmId = req.body.staffId;
  var streamName = 'project-' + id;
  client.writeEvent(streamName, 'pmToProjectAdded', { id: id, pmId: pmId})
    .then(function(){
      console.log('Pm to project added event written');
      res.end();
    });
});

app.get('/', function (req, res) {
  var name = process.env.MY_NAME || 'unknown';
  res.send('Express JS says "Hello ' + name + '"!');
});

var products = [
  {"id": 1, "name": "Apples"},
  {"id": 2, "name": "Pears"},
  {"id": 3, "name": "Lemons"}
];

app.get('/products', function (req, res){
  res.send(products);
});

app.get('/products/:id', function (req, res){
  res.send(products[req.params.id]);
});

var port = process.env.PORT || 1337;
app.listen(port, function(){
  console.log("Express server listening on port %d in %s mode", port, app.settings.env);
});
