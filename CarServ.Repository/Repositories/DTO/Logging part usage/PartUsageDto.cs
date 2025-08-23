using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Logging_part_usage
{
    public class PartUsageDto
    {
        public int PartID { get; set; }
        public int ServiceID { get; set; }
        public int QuantityUsed { get; set; }
    }
    public class PartDto
    {
        public int PartId { get; set; }
        public string PartName { get; set; }
        public int? Quantity { get; set; }
        public decimal? CurrentUnitPrice { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public int? WarrantyMonths { get; set; }
        public string SupplierName { get; set; } 
        public List<PartPriceDto> PartPrices { get; set; } = new List<PartPriceDto>();
    }

    public class PartPriceDto
    {
        public decimal Price { get; set; }
        public DateTime? EffectiveFrom { get; set; }
    }


}
