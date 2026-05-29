# WebMassTransit Example

Demonstrates sending emails via **useSend** through a [MassTransit](https://masstransit.io/) message consumer.

## Why MassTransit?

Publishing a `SendEmailCommand` decouples the caller from the email provider. The consumer can be moved to a separate worker service and swapped between transports (RabbitMQ, Azure Service Bus, Amazon SQS) without changing the publisher.

## Setup

```bash
cd examples/WebMassTransit
dotnet user-secrets set "UseSend:ApiToken" "us_your_token"
dotnet run
```

## Try it

```bash
curl -X POST http://localhost:5000/send \
  -H "Content-Type: application/json" \
  -d '{"to":"you@example.com","subject":"Hello","htmlBody":"<p>Hi!</p>"}'
```

## Key patterns

| Pattern | Detail |
|---------|--------|
| **Command message** | `SendEmailCommand` record — serialisable, transport-agnostic |
| **Consumer** | `SendEmailCommandConsumer` — injected with `IUseSend` from DI |
| **Transport** | In-memory for this demo; change one line for RabbitMQ/ASB/SQS |
| **Error handling** | Throwing inside a consumer triggers MassTransit's retry/fault pipeline |
