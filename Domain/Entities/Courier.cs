using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Courier : BaseEntity
    {
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public DateTime Birthdate { get; set; }
        public string Cnh { get; set; }
        public CnhType CnhType { get; set; }
        public string? CnhImage { get; set; }

        [JsonIgnore]
        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        [JsonIgnore]
        public virtual ICollection<Order> Notifications { get; set; } = new List<Order>();

        public Courier(string name, string cnpj, DateTime birthDate, string cnh, CnhType cnhType) : base()
        {
            Name = name;
            Cnpj = cnpj;
            Birthdate = birthDate;
            Cnh = cnh;
            CnhType = cnhType;
        }
    }

    public enum CnhType
    {
        A,
        B,
        AB
    }
}
