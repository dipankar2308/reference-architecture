# Reference Architecture diagram

``mermaid
C4Container
    title Clean Architecture + Hexagonal Pattern

    Person(user, "End User", "Business user")

    Container(WebAPI, "Web API", "ASP.NET Core", "REST API entry point")
    Container(Application, "Application Layer", ".NET", "Use cases, ports, MediatR handlers")
    Container(Domain, "Domain Layer", ".NET", "Entities, business rules, value objects")
    Container(Infrastructure, "Infrastructure", ".NET", "Azure Cosmos DB, Service Bus adapters")

    Rel(user, WebAPI, "HTTPS", "uses")
    Rel(WebAPI, Application, "calls", "dependency injection")
    Rel(Application, Domain, "depends on", "domain services")
    Rel(Application, Infrastructure, "uses ports", "IProductRepository, IEventPublisher")
```
