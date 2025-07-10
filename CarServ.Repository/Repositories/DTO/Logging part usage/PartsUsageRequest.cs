using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Logging_part_usage
{
    public class PartsUsageRequest
    {
        public int ServiceID { get; set; }        
        public List<PartUsageDto> PartsUsed { get; set; }
        public string Notes { get; set; }
    }
}
