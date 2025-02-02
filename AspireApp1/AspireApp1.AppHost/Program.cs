var builder = DistributedApplication.CreateBuilder(args);


// SQL doesn't support any auto-creation of databases or running scripts on startup so we have to do it manually.
var sql = builder
    .AddSqlServer("alloy-sql")
    // Mount the init scripts directory into the container.
    .WithBindMount(@".\sqlserverconfig", "/usr/config")
    // Mount the SQL scripts directory into the container so that the init scripts run.
    .WithBindMount(@"..\..\AlloyCms12New\App_Data\sqlserver", "/docker-entrypoint-initdb.d")
    // Run the custom entrypoint script on startup.
    .WithEntrypoint("/usr/config/entrypoint.sh")
    // Configure the container to store data in a volume so that it persists across instances.
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("EPiServerDB", databaseName: "alloy-db-cms");
builder
    .AddProject("alloy-cms", @"..\..\AlloyCms12New\AlloyCms12New.csproj")
    .WithReference(db)
    .WaitFor(db);

await builder.Build().RunAsync();
