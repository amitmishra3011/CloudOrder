using System;

namespace CloudOrder.Domain.Entities
{
    public class Order
    {
        public Guid ID { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "Created";
    }
}