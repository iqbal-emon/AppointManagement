namespace AppointManagement.Models.DTO
{
    public class CreateUpdateAppointmentDTO
    {
        public string? PatientName { get; set; }
        public string? PatientContactInfo { get; set; }
        public DateTime? AppointmentDateTime { get; set; }
        public int DoctorId { get; set; }
    }

}
