using Grpc.Core;
using Contracts.Currency;

namespace CurrencyService.GrpcServices;

public class CurrencyServiceImpl : CurrencyGrpcService.CurrencyGrpcServiceBase
{
    private readonly List<Currency> _currencies = new List<Currency>
    {
        new Currency { Id = "1", Name = "One" },
        new Currency { Id = "2", Name = "Two" },
        new Currency { Id = "3", Name = "Three" },
    };

    public override async Task<CurrencyList> GetCurrencies(Empty request, ServerCallContext context)
    {
        await Task.Delay(100);

        return new CurrencyList { Currencies = { _currencies } };
    }

    public override async Task<CurrencyList> GetCurrenciesByIds(CurrencyRequest request, ServerCallContext context)
    {
        await Task.Delay(100);
        
        return new CurrencyList { Currencies = { _currencies } };
    }
}
