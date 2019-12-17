# GraphAPI

This project is an experimental proof of concept, combining technologies and methodologies(not religiously) in order to show new ideas and concepts.
It is a simple implementation of a task manager, including User, Project and Task management.
There is inspiration on DDD, Hexagonal Architecture and CQRS.

### Tech

GraphAPI uses a number of open source projects and tools to work properly:

* [.NET Core 3.0]()
* [ASP NET Core 3.0]() 
* [Entity Framework Core 3.0]() 
* [GraphQL](https://github.com/graphql-dotnet) 
* [GraphQL Playground](https://github.com/graphql-dotnet/server) 
* [MassTransit](https://github.com/MassTransit/MassTransit) 
* [Automapper](https://github.com/AutoMapper/AutoMapper)
* [MediatR](https://github.com/jbogard/MediatR)
* [RabbitMQ]() - Default
* [Azure Service Bus]() - Optional
* [MongoDB]() - Default
* [PostgreSQL]() - Default
* [Microsoft SQL Server]() - Optional
* [Docker]()

 
##### Soon
* [ElasticSearch]() 
* [Kubernetes]() 

### Installation

GraphAPI can be started using Visual Studio with a properly stack configuration on appSettings.json.
There is, also, a docker compose file with the full stack ready to be started.

##### Docker Compose
By default the ports 27017 (MongoDB), 5432(PostgreSQL), 5672(RabbitMQ), <del>5017(HTTPS, requires SSL certificate)</del>, 5016 (HTTP).

```sh
$ cd {ROOT_PROJECT}
$ docker-compose up -d
```

Then, open it on <http://localhost:5006/ui/playground> or <https://localhost:5007/ui/playground> (if you configured HTTPS) and enjoy.

### Development

Want to contribute? Great!

Start development on this project is simple, just open it on VSCode, Visual Studio or your preferred IDE and start to develop. 

### Todos

 - Write MORE Tests
 - Implement Elastic Search Manager
 - Kubernetes Implementation
 - Add Notes on Tasks

License
----

MIT