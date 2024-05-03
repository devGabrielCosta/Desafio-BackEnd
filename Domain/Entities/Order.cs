using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public decimal DeliveryFee {  get; set; }
        public Status Status { get; set; }

        public Guid? CourierId { get; set; }

        [JsonIgnore]
        public virtual Courier Courier { get; set; } = null!; 

        [JsonIgnore]
        public virtual ICollection<Courier> Notifieds { get; set; } = new List<Courier>();

        public Order(decimal deliveryFee)
        {
            CreatedAt = DateTime.Now;
            Status = Status.Available;
            DeliveryFee = deliveryFee;
        }

    }

    public enum Status
    {
        Available,
        Accepted,
        Delivered,
    }
}
