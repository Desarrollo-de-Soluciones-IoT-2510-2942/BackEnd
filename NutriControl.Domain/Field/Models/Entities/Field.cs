using Domain;

namespace NutriControl.Domain.Fields.Models.Entities;

public class Field: ModelBase
{
    public int UserId { get; set; }   
    public string Name { get; set; } 
    public string Location { get; set; } 
    public string SoilType { get; set; } 
    public double Elevation { get; set; }
}