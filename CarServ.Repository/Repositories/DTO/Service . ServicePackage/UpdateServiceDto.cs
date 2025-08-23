using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Service_._ServicePackage
{
    public class UpdateServiceDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? EstimatedLaborHours { get; set; }
        public List<ServicePartDto> ServiceParts { get; set; } 
    }

    public class ServicePartDto
    {
        public int PartId { get; set; }
        public int QuantityRequired { get; set; }
    }

}
