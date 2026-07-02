namespace CloudOrder.Entities.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public decimal TotalAmount { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<OrderItem> Items { get; set; }
            = new List<OrderItem>();

    }
}
