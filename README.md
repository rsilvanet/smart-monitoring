# Smart Monitoring

## Endpoints

* `GET /services` : Retrieves all the services.
* `GET /services/foobar` : Retrieves foobar service.
* `GET /services?labels=groups:apis` : Retrieves all the services with a `groups:apis` label.
* `POST /services/foobar` : Adds a new service, using a json body with the information.
* `PUT /services/foobar` : Updates an existing service using a json body with the new information.
* `DELETE /services/foobar` : Deletes an existing service.

## Fields

* `name` : The service name, must be unique (should be a name from 4 to 30 characters).
* `port` : Which port the service should run on (should be a valid port).
* `maintainer` : The person that is responsible for the service (should be a valid email).
* `labels` : Can be multiple labels, following a `key:value` convention.

## Technical decisions

The API was developed using ASP.NET Core 5.0 and Entity Framework Core to map and manage the entities in a SQL database. The database configured is actually an in-memory database simply to make it easier to run the project with no external dependencies. Including a docker image for any SQL database would also be an option and the switch would have almost no impact on the implementation.

The code is using DDD concepts to make rich domain models by using pre-defined value objects and by encapsulating methods and attributes properly, preventing entities from getting into an invalid state for the application. Business logic for the operations that can be executed over the entities are organized in separate use cases that are responsible for doing one thing and one thing only.

The repository pattern is used for managing the database resources. The repository interfaces are declared in the business project level (see project structure below) and can be implemented by other projects in the solution depending on the desired database implementation. You simply need to hook the correct implementation to the interface when configuring the dependency injector of the application.

Unit tests were added for the different layers of the application and are written using the XUnit framework.

## Project structure

* **SmartMonitoring.Domain**: Contains the entities, value objects and any other definitions that are considered a core part of the application. Entities and value objects will contain proper validation to ensure they won't get into a bad state such as an email field holding a value which is not a valid email address for example.

* **SmartMonitoring.Business**: Contains the services, use cases and any application logic that needs to interact with other components such as a database or API. For those interactions interfaces are declared to abstract the functionality expected from those components which will be then implemented outside of the business scope.

* **SmartMonitoring.MemoryDatabase**: Contains the implementation and configuration necessary to use Entity Framework to connect to an in-memory database. Repository interfaces defined in the business level are implemented in this scope and such implementations are later on injected via dependency injection on the API scope.

* **SmartMonitoring.API**: Contains the controller definitions that will become the API endpoints previously described. The controller actions will call the necessary use cases and services from the business scope that were injected at the startup.

## How to run

The application can be ran with a simple `dotnet run` from `src/SmartMonitoring.API` but there's also a `Dockerfile` that can be ran with a `docker compose up` from the root level of the repository where the `docker-compose.xml` is placed. 

Both commands will start the application on `http://localhost:5000`. If ran with the `dotnet run` it will also make an https available on `https://localhost:5001`. A swagger definition can be found by navigating to `/swagger`. A postman collection is also available inside the `postman` directory which has all the available requests pre-configured to be executed against the localhost API.

![image](https://user-images.githubusercontent.com/9820133/133346902-226b4a29-9c70-4b9a-b9cf-10d340ba9c6e.png)

## Possible improvements

* In a development environment it would probably be better to configure a database with a docker image instead of using an in-memory database which is reseted every time the application is restarted. The `docker-compose.xml` could be extended to also create a container for the database image then.

* There are arguments for not throwing exceptions when dealing with expected validation errors as it's done in some domain entities and value objects. This can result in performance for the application depending on the load and on how often such exceptions would be raised. The payoff is that the custom descriptive exceptions are easy to read and understand in the code, but they could be replaced by a different solution that would deal with the errors as messages that can be read later instead. In the end, it is a design decision that needs to be made.

* The service tags in the database are not reused in case they already exist. That means that if `service1` is created with the tag `groups:apis` and `service2` is later added with the same tag, two separate entries will be created in the database. This is also a design decision that can have an impact depending on how these tags are used by the system.

* The postman collection could be extend to include some tests and serve as an integration test suite for the API layer. Since the application is configured to run with docker, such tests could even be ran in some build pipeline together with the unit tests.
