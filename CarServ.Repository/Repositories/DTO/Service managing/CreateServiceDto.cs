using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Service_managing
{
    public class CreateServiceDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? EstimatedLaborHours { get; set; }
    }

    public class CreateServicePackageDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public List<int> ServiceIds { get; set; } = new List<int>();
    }

}
