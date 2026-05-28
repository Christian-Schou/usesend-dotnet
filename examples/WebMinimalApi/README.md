# WebMinimalApi Example

Demonstrates the useSend .NET SDK in an ASP.NET Core minimal API application with dependency injection.

## Endpoints

| Method | Path       | Description                |
|--------|------------|----------------------------|
| `POST` | `/send`    | Send a transactional email |
| `GET`  | `/domains` | List verified domains      |

## Run

```bash
export USESEND_API_KEY=us_your_api_token
dotnet run
```

Or set `UseSend:ApiToken` in `appsettings.json`.

### Self-Hosted

Set `UseSend:ApiUrl` in `appsettings.json`:

```json
{
  "UseSend": {
    "ApiToken": "us_your_token",
    "ApiUrl": "https://send.mycompany.com/api/"
  }
}
```

### Send an email

```bash
curl -X POST http://localhost:5000/send \
  -H "Content-Type: application/json" \
  -d '{"from":"noreply@yourdomain.com","to":"user@example.com","subject":"Hello","text":"Hi!"}'
```
