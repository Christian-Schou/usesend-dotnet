# WebTemporal Example

Demonstrates sending emails via **useSend** inside a durable [Temporal](https://temporal.io/) workflow.

## Why Temporal?

Temporal provides durable execution with built-in retries, timers, and visibility. Email steps that span days (e.g., send welcome → wait 3 days → send follow-up) are expressed as simple sequential code — Temporal handles persistence and recovery automatically.

## Prerequisites

Install and start the Temporal dev server:

```bash
# Install Temporal CLI
brew install temporal

# Start the local dev server
temporal server start-dev
```

## Setup

```bash
cd examples/WebTemporal
dotnet user-secrets set "UseSend:ApiToken" "us_your_token"
dotnet run
```

## Starting a workflow

Use the Temporal CLI to start a workflow run:

```bash
temporal workflow start \
  --type WelcomeEmailWorkflow \
  --task-queue email-task-queue \
  --input '{"To":"you@example.com","Name":"Alice"}'
```

Or open the Temporal Web UI at `http://localhost:8233` to inspect workflow history.

## What the workflow does

1. Sends a **welcome email** immediately.
2. Waits **3 days** (durable timer — survives restarts).
3. Sends a **follow-up email** after the timer fires.

## Key patterns

| Pattern | Detail |
|---------|--------|
| **Activity** | `[Activity]` methods injected with `IUseSend` from DI |
| **Workflow** | `[Workflow]` class orchestrates activities + timers |
| **Retry** | Temporal retries failed activities automatically |
| **Durable timer** | `Workflow.DelayAsync(TimeSpan.FromDays(3))` — no cron needed |
