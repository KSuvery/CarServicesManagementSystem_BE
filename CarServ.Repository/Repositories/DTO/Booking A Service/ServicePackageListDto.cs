using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Booking_A_Service
{
    public class ServicePackageListDto
    {
        public List<ServicePackageDto> Packages { get; set; }
        public DateTime CurrentDate { get; set; }
    }
    public class ServiceListDto
    {
        public List<ServiceDto> Services { get; set; }
        public DateTime CurrentDate { get; set; }
    }
}
