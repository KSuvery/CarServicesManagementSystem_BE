namespace CarServ.Repository.Repositories.DTO.Booking_A_Service
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string StaffName { get; set; }
        public string VehicleLicensePlate { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public int VehicleYear { get; set; }
        public List<string> services { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public TimeOnly BookedTime { get; set; }
        public string Status { get; set; }
    }

}
