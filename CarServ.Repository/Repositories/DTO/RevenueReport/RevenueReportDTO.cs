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
        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }

    public class DailyServicesRevenueReportDto
    {
        public DateTime Date { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderLineItemDto> LineItems { get; set; } = new List<OrderLineItemDto>();
    }

    public class OrderLineItemDto
    {
        //CHỗ này phải sửa lại thành Item Name
        public int Item { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }


    public class DashboardSummaryDto
    {
        public Dictionary<string, int> PartsUsage { get; set; } = new Dictionary<string, int>(); // PartName -> Quantity
        public Dictionary<string, int> ServicesCount { get; set; } = new Dictionary<string, int>(); // ServiceName -> Count
        public Dictionary<string, int> PackagesCount { get; set; } = new Dictionary<string, int>(); // PackageName -> Count
    }

    public class DashboardSummaryInVehiclesDto
    {
        public Dictionary<string, int> VehicleServicesCount { get; set; } = new Dictionary<string, int>(); // VehicleModel -> Count
    }
}
