namespace WarehouseApp.Entities;

public class Ski : Equipment
{
   
    public string? SkiFlex { get; set; }

    public override string ToString() => $"Id: {Id}, Type: {Type}, Name: {Name}, ListPrice: {ListPrice}, Color: {Color}, ski flex: {SkiFlex}";
}