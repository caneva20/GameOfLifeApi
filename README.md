### Project setup:
1. Install .NET Core SDK 7.0
2. Update appsettings.Development.json with your connection string
```shell
"ConnectionStrings": {
  "Default": "Server={SERVER};Database=GameOfLife;User Id={USER};Password={PASSWORD};TrustServerCertificate=True"
}
```
3. Run `dotnet ef database update` to apply migrations
   > Installing EF Core CLI tools might be required: `dotnet tool install --global dotnet-ef --version 7.0.20`
4. Run `dotnet run --project GameOfLife.Api`
5. Navigate to http://localhost:5000