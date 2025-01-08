using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace AppointManagement.Models
{
    public class Appointment
    {
        
        public int AppointmentId { get; set; }
        [Required]
        public string? PatientName { get; set; }
        [Required]
        public string? PatientContactInfo { get; set; }
        [Required]
        public DateTime? AppointmentDateTime { get; set; }
        [Required]
        [ForeignKey("Doctor")]

        public int? DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        

    }
}
