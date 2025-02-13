namespace Program;

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

// Load MongoDB settings from configuration
        var mongoSettings = new MongoDbSettings
        {
            ConnectionString = builder.Configuration.GetConnectionString("MongoDb"),
            DatabaseName = builder.Configuration["MongoDb:DatabaseName"]
        };

        builder.Services.AddSingleton(mongoSettings);
        builder.Services.AddSingleton<UserRepository>();

        var app = builder.Build();
        app.Run();
    }
}