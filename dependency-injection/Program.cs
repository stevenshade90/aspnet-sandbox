var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddScoped<IAccountReader, AccountReader>();
builder.Services.AddScoped<BankAccount>();

var app = builder.Build();
app.MapRazorPages();

app.MapGet("/account/{name}", ReadInterestRate);

app.Run();

void ReadInterestRate(string username, IAccountReader p)
{
    Console.WriteLine($"Displaying interest rate for {username}");
    p.DisplayInterest();
}

// Service implementation (DI container will create this class)
public class AccountReader : IAccountReader
{ 
    private readonly BankAccount _bank;

    public void DisplayInterest()
    {
        Console.WriteLine($"\t{_bank.GetInterest()}");
    }

    public AccountReader(BankAccount ba)
    { 
        _bank = ba;
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

// Service type
public interface IAccountReader
{
    void DisplayInterest();
}