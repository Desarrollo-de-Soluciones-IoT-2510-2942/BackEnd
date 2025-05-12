

namespace Domain;

public class Subscription: ModelBase
{
    public int UserId { get; set; }           
    public PlanType PlanType { get; set; }      
    public DateTime StartDate { get; set; }      
    public DateTime EndDate { get; set; }        
}

public enum PlanType
{
    Basic,
    Standard,
    Premium
}
