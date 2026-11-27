namespace configuration_builder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Configuration.Sources.Clear();
            builder.Configuration.AddJsonFile("appsettings.json", optional: true);
            //builder.Configuration.AddEnvironmentVariables();
            builder.Configuration.AddUserSecrets<Program>();

            WebApplication app = builder.Build();


            app.MapGet("/", (IConfiguration config) => config.AsEnumerable());
            app.MapGet("/GetName", (IConfiguration config) =>
            {
                string name = config["CustomSettings:DefaultName:first"];
                return name;
            });
            app.MapGet("/GetSecret", (IConfiguration config) =>
            {
                return config.GetSection("UserId")["secretVal"];
            });

            app.Run();
        }
    }
}