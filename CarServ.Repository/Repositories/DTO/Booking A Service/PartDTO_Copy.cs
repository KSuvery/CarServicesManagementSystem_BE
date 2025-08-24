using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Booking_A_Service
{
    public class PartDTO_Copy
    {
        public int PartId { get; set; }
        public string PartName { get; set; }
        public int QuantityRequired { get; set; }
        public decimal? UnitPrice { get; set; }
    }

}
