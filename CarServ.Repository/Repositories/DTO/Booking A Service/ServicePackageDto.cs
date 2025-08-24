using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Booking_A_Service
{
    public class ServicePackageDto
    {
        public int PackageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public List<ServiceDto> Services { get; set; }

    }
    public class ServiceDto
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? EstimatedLaborHours { get; set; }
        public List<PartDTO_Copy> Parts { get; set; }
    }
}
