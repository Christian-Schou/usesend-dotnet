# WebHangfire Example

Demonstrates sending transactional emails via **useSend** as background [Hangfire](https://www.hangfire.io/) jobs.

## Why Hangfire?

Fire-and-forget background jobs decouple email delivery from HTTP request latency. If the job fails, Hangfire retries automatically.

## Setup

```bash
cd examples/WebHangfire
dotnet user-secrets set "UseSend:ApiToken" "us_your_token"
dotnet run
```

## Try it

```bash
# Enqueue a welcome email job
curl -X POST "http://localhost:5000/send-welcome?to=you@example.com"

# View Hangfire dashboard
open http://localhost:5000/hangfire
```

## Key patterns

- **`EmailJob`** — a plain C# class injected by Hangfire with `IUseSend` from DI.
- **`IBackgroundJobClient.Enqueue`** — schedules the job fire-and-forget.
- **Retry** — Hangfire retries failed jobs up to 10 times by default.
- **Storage** — swap `UseInMemoryStorage()` for `UseSqlServerStorage()` or `UseRedisStorage()` in production.
