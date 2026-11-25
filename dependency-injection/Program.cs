var builder = WebApplication.CreateBuilder(args);

// Registering with DI container
builder.Services.AddRazorPages();
builder.Services.AddAccountReader();

var app = builder.Build();
app.MapRazorPages();

app.MapGet("/account/{username}", ReadInterestRate);

app.Run();

string ReadInterestRate(string username, IAccountReader p)
{
    string interestText = p.DisplayInterest();
    return $"Account info for: {username}\n{interestText}";
}

// Service implementation (DI container will create this class)
public class AccountReader : IAccountReader
{ 
    private readonly BankAccount _bank;

    public AccountReader(BankAccount ba)
    { 
        _bank = ba;
    }
    public string DisplayInterest()
    {
        return _bank.GetInterest();
    }
}

// Dependency
public class BankAccount
{
    private static string _company = "PNC";
    private static double _interest = 2.00;

    public string GetInterest()
    {
        return $"Current interest rate for {_company} bank is {_interest}%";
    }
}

public static class AccountReaderServiceCollectionExtensions
{ 
    public static IServiceCollection AddAccountReader(this IServiceCollection services)
    {
        services.AddScoped<IAccountReader, AccountReader>();
        services.AddScoped<BankAccount>();
        return services;
    }
}

// Service type
public interface IAccountReader
{
    string DisplayInterest();
}