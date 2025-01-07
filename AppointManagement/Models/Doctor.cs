using System.ComponentModel.DataAnnotations;

namespace AppointManagement.Models
{
    public class Doctor
    {
      
         public int DoctorId { get; set; }
        [Required]
        public string? DoctorName { get; set; }
     

    }
}
