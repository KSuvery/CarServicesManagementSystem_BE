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
}
