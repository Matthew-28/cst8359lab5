using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab5.Models
{
    public class Deal
    {
        public int DealId { get; set; }

        public String ImageLink { get; set; }

        public String FoodDeliveryServiceId { get; set; }

        [ForeignKey("FoodDeliveryServiceId")]
        public virtual FoodDeliveryService FoodDeliveryService { get; set; }
}
}
