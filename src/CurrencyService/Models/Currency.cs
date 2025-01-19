namespace CurrencyService.Models;

public class Currency
{
    public required string Id { get; set; }
    public required string CharCode { get; set; }
    public required string Name { get; set; }
    public int Nominal { get; set; }
    public long Rate { get; set; }
    public DateTime UpdatedAt { get; set; }
}
