# ADR 002: Event-Driven Communication Between Services

## Status

Accepted

## Context

Synchronous REST calls between microservices create tight coupling and cascading failures.

## Decision

Use Azure Service Bus with Topics and Subscriptions for cross-service communication.
Use MassTransit as the messaging abstraction layer.

## Consequences

GOOD: Services decoupled and independently deployable
GOOD: Resilient to temporary service outages
GOOD: Audit trail via message logs
BAD: Eventual consistency must be handled in domain logic
