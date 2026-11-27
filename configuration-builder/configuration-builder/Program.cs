using Microsoft.Extensions.Options;

namespace configuration_builder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Configuration.Sources.Clear();
            builder.Configuration
                .AddJsonFile("appsettings.json", 
                optional: true,
                reloadOnChange: true);
            //builder.Configuration.AddEnvironmentVariables();
            builder.Configuration.AddUserSecrets<Program>();

            builder.Services.Configure<UserInfo>(builder.Configuration.GetSection("UserInfo"));

            // DI alternative
            builder.Services.AddSingleton<UserInfo>(provider =>
                provider.GetRequiredService<IOptions<UserInfo>>().Value);

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
            app.MapGet("/GetProfession", (IOptions<UserInfo> options) =>
            {
                UserInfo user = options.Value;
                return user.Profession;
            });
            app.MapGet("/GetProfessionWithDi", (UserInfo info) => info.Profession);

            app.Run();
        }
    }

    public class UserInfo
    {
        public string? Age { get; set; }
        public string? Profession {  get; set; }
        public string? Salary {  get; set; }

        public UserInfo() { }
    }
}