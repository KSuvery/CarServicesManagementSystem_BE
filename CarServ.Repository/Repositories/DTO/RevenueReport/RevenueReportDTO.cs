using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.RevenueReport
{
    public class RevenueReportDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalPayments { get; set; }
        public decimal TotalPaymentAmount { get; set; }
        public int TotalPartsUsed { get; set; }
        public Dictionary<int, int> PartsQuantity { get; set; } = new Dictionary<int, int>();
    }


}
