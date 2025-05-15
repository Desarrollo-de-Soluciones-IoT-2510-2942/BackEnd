namespace Domain;

public class Recommendation : ModelBase
{
    public int CropId { get; set; }
    public string Content { get; set; } 
    public string Type { get; set; } 
    public int Priority { get; set; } 
}