# CompStats

Mount & Blade II: Bannerlord server mod for exporting player statistics and match (mission) data

## For development

In the solution's bin directory, create a "taleworlds" directory and copy all the needed TaleWorlds DLLs from your `Mount and Blade II: Dedicated Server/bin/Linux64_Shipping_Server` directory

Verify that the file paths match with the dependencies in src/CompStats.csproj

## How to use

At the moment, the mod does not save any data directly to a database; instead it will export all data to an external API. So to use it, you need your own API with a `POST /stats` endpoint and then you can handle the data however you want. **This might not be the case after further testing or future requirement changes, this is still a work in progress.**

Create a JSON file "config.json" in `/bin/Linux64_Shipping_Server/config.json`, in it you can set your base API URL, your API key, your current running tournament name and if stats tracking should be enabled or not. An example of this config file:

```
{
  "enabled": true,
  "apiURL": "http://myapi.com",
  "apiKey": "myapikey123",
  "tournamentName":  "My Tournament"
}
```

