namespace WarehouseApp.Entities;

public class Snowboard : Equipment
{
    public string? SnowboardFlex { get; set; }

    public override string ToString() => $"Id: {Id}, Type: {Type}, Name: {Name}, ListPrice: {ListPrice}, Color: {Color}, ski flex: {SnowboardFlex}";
}