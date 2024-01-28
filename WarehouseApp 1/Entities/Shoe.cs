namespace WarehouseApp.Entities
{
    public class Shoe : Equipment
    {
        public int FeetSize { get; set; }
        public override string ToString() => $"Id: {Id}, Person: {Person}, feet size: {FeetSize},";
    }
}