## Testing Multiplayer with Ngrok

1. Install ngrok.
2. Execute the start-up code for NGROK on your assigned port. For example:
```
ngrok http --region=us --host-header=localhost --hostname=rlengine.ngrok.io https://localhost:7083
```

Deploy to Azure 3

Add Migration
```
dotnet ef migrations add Initial-Migration --project RLEngine.Server
```