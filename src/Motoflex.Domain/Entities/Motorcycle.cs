namespace Motoflex.Domain.Entities
{
    public class Motorcycle(int year, string model, string licensePlate) : BaseEntity()
    {
        public int Year { get; set; } = year;
        public string Model { get; set; } = model;
        public string LicensePlate { get; set; } = licensePlate;
        public bool Available { get; set; } = true;
    }
}
