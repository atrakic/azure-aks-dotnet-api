namespace DotnetApi.Services;

public interface IQuoteService
{
    Task<string> RandomQuote();
}

public class QuoteService : IQuoteService
{
    public Task<string> RandomQuote()
    {
        List<string> Quotes = new()
        {
            "The only way to get started is to quit talking and begin doing.",
            "The pessimist complains about the wind; the optimist expects it to change; the realist adjusts the sails.",
            "Don’t let yesterday take up too much of today.",
            "You learn more from failure than from success. Don’t let it stop you. Failure builds character.",
            "It’s not whether you get knocked down, it’s whether you get up."
        };

        var quote = Quotes[new Random().Next(Quotes.Count)];
        return Task.FromResult(quote);
    }
}
