namespace AppointManagement.Models.DTO
{
    public class AppointmentDetailsDTO
    {
        public int AppointmentId { get; set; }
        public int? doctorId { get; set; }
        public string? PatientName { get; set; }
        public string? PatientContactInfo { get; set; }
        public DateTime? AppointmentDateTime { get; set; }
        public string? DoctorName { get; set; }
    }

}
