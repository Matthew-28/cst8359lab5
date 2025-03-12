using Microsoft.EntityFrameworkCore;

namespace Lab5.Models
{
    [PrimaryKey(nameof(CustomerId), nameof(FoodDeliveryServiceId))]
    public class Subscription
    {
        public int CustomerId { get; set; }

        public String FoodDeliveryServiceId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual FoodDeliveryService FoodDeliveryService { get; set; }
    }
}
