using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Service_._ServicePackage
{
    public class UpdateServicePackageDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public List<int> ServiceIds { get; set; } // For updating services in the package
    }

}
