namespace configuration_builder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Configuration.Sources.Clear();
            builder.Configuration.AddJsonFile("appsettings.json", optional: true);

            WebApplication app = builder.Build();

            app.MapGet("/", (IConfiguration config) => config.AsEnumerable());
            app.MapGet("/GetName", (IConfiguration config) =>
            {
                string name = config["CustomSettings:DefaultName:first"];
                return name;

            });

            app.Run();
        }
    }
}