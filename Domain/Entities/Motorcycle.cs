using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Motorcycle : BaseEntity
    {
        public int Year { get; set; }
        public string Model {  get; set; }
        public string LicensePlate { get; set; }
        public bool Available { get; set; }

        [JsonIgnore]
        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
        public Motorcycle(int year, string model, string licensePlate) : base()
        {
            Year = year;
            Model = model;
            LicensePlate = licensePlate;
            Available = true;
        }
    }
}
