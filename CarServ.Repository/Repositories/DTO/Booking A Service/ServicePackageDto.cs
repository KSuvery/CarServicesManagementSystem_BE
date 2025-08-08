using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Booking_A_Service
{
    public class ServicePackageDto
    {
        public int PackageID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; } // Null if no promotion
        public string PromotionTitle { get; set; } // Null if no promotion
        public bool HasPromotion => DiscountedPrice.HasValue;
    }
}
