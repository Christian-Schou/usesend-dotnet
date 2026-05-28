# ConsoleNoDi Example

Demonstrates using the useSend .NET SDK without dependency injection.

## Run

```bash
export USESEND_API_KEY=us_your_api_token
dotnet run
```

Or pass the token directly in `Program.cs`:

```csharp
var client = UseSendClient.Create("us_your_api_token");
```
