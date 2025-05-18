using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.OrderEntities
{
    public class Order: BaseEntity<Guid>                       
    {
        public Order()
        {
            
        }

        public Order(string buyEmail, 
            Address shippingAddress, 
            ICollection<OrderItem> orderItems, 
            
            DeliveryMethod deliveryMethod, 
           
            decimal subTotal)
        {
            BuyEmail = buyEmail;
            ShippingAddress = shippingAddress;
            OrderItems = orderItems;
            
            DeliveryMethod = deliveryMethod;
           
            SubTotal = subTotal;
        }

        public string BuyEmail { get; set; }

        public Address ShippingAddress { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public OrderPaymentStatus PaymentStatus { get; set; }= OrderPaymentStatus.Pending;

        public  DeliveryMethod DeliveryMethod { get; set; }
    
        public int? DeliveryMethodId { get; set; }

        public DateTimeOffset OrderDate { get; set; }= DateTimeOffset.Now;

        public decimal SubTotal { get; set; }
    }
}
