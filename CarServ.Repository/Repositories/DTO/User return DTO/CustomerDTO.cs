using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.User_return_DTO
{
    public class CustomerDTO
    {
        public int UserID { get; set; }
        //public string FullName { get; set; }
        public string Email { get; set; }
        //public string PhoneNumber { get; set; }
        //public string RoleName { get; set; }
        //public string Address { get; set; }
    }
    public class CustomerWithVehiclesDTO
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public string Address { get; set; }
        public List<Vehicle> vehicles { get; set; }
    }
    public class UpdateProfileDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

}
