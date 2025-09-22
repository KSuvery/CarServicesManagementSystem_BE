using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO
{
    public class CustomerDashboard
    {
        public int TotalServices { get; set; }
        public int CompletedServices { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
