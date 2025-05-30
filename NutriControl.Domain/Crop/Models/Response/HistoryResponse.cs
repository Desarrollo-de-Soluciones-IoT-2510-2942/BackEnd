namespace _1_API.Response;

public class HistoryResponse
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string SavingsType { get; set; }
    public decimal AmountSaved { get; set; }
    public string UnitOfMeasurement { get; set; }
    public double PercentageSaved { get; set; }
}