namespace Motoflex.Domain.Entities
{
    public class Renter(string name, string cnpj, DateTime birthDate, string cnh, CnhType cnhType) : BaseEntity()
    {
        public string Name { get; set; } = name;
        public string Cnpj { get; set; } = cnpj;
        public DateTime Birthdate { get; set; } = birthDate;
        public string Cnh { get; set; } = cnh;
        public CnhType CnhType { get; set; } = cnhType;
        public string? CnhImage { get; set; }
    }

    public enum CnhType
    {
        A,
        B,
        AB
    }
}
