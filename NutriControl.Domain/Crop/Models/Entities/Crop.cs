

namespace Domain;

public class Crop: ModelBase
{
    public int FieldId { get; set; }
    public string CropType { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; }
    
}