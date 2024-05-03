using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Rental : BaseEntity
    {
        public Plan Plan { get; set; }
        public Guid CourierId { get; set; }
        public Guid MotorcycleId { get; set; }
        public DateTime BeginAt { get; set; }
        public DateTime FinishAt { get; set; }
        public DateTime ReturnAt { get; set; }
        public bool Active { get; set; }

        [JsonIgnore]
        public virtual Motorcycle Motorcycle { get; set; } = null!;
        [JsonIgnore]
        public virtual Courier Courier { get; set; } = null!;

        public Rental(Plan plan, Guid courierId) : base()
        {
            Plan = plan;
            CourierId = courierId;
            BeginAt = DateTime.Now.AddDays(1);
            Active = true;

            if (Plan is Plan.A)
                FinishAt = BeginAt.AddDays(6);
            else if (Plan is Plan.B)
                FinishAt = BeginAt.AddDays(14);
            else if (Plan is Plan.C)
                FinishAt = BeginAt.AddDays(29);
        }

    }

    public enum Plan
    {
        A,
        B,
        C
    }
}
