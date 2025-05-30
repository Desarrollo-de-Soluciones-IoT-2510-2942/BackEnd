namespace Domain;

public class History: ModelBase
{
    public int CropId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string SavingsType { get; set; }
    public decimal AmountSaved { get; set; }
    public string UnitOfMeasurement { get; set; }
    public double PercentageSaved { get; set; }
    
}