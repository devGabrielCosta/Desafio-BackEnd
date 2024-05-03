namespace Domain.Handlers.Commands
{
    public class NotifyOrderCouriersCommand
    {
        public Guid OrderId { get; set; }
        public NotifyOrderCouriersCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
