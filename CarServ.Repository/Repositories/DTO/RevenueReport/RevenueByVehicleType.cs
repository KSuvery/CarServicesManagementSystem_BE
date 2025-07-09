using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.RevenueReport
{
    public class RevenueByVehicleType
    {
        public string VehicleType { get; set; }
        public decimal Revenue { get; set; }
        public int VehicleCount { get; set; }
    }
}
