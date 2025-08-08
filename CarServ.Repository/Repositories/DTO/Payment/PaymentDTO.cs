using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Payment
{
    public class PaymentDto
    {
        public int AppointmentId { get; set; }
        public string PaymentMethod { get; set; }
    }
}
