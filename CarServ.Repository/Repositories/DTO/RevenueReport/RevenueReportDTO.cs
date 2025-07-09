using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.RevenueReport
{
    public class RevenueReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal ServiceRevenue { get; set; }
        public decimal PartsRevenue { get; set; }
        public List<RevenueByPackage> RevenueByPackages { get; set; }
        public List<RevenueByVehicleType> RevenueByVehicleTypes { get; set; }
    }
}
