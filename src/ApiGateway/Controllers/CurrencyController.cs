using Microsoft.AspNetCore.Mvc;
using Contracts.Currency;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("currency")]
public class CurrencyController : ControllerBase
{
    private readonly CurrencyGrpcService.CurrencyGrpcServiceClient _currencyServiceClient;

    public CurrencyController(CurrencyGrpcService.CurrencyGrpcServiceClient currencyServiceClient)
    {
        _currencyServiceClient = currencyServiceClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrencies()
    {
        var grpcResponse = await _currencyServiceClient.GetCurrenciesAsync(new Empty());
        return Ok(grpcResponse.Currencies.Select(c => new
        {
            c.Id,
            c.Name
        }));
    }
}
