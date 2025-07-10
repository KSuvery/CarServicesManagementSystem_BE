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
}
