using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.RevenueReport
{
    public class RevenueByPackage
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public decimal Revenue { get; set; }
        public int ServiceCount { get; set; }
    }
}
