using AppointManagement.Context;
using AppointManagement.Models;
using AppointManagement.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointManagmentController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AppointManagmentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("appointments")]
        [Authorize]
        public IActionResult CreateAppointment([FromBody] CreateUpdateAppointmentDTO appointmentDto)
        {
            if (string.IsNullOrWhiteSpace(appointmentDto.PatientName))
            {
                return BadRequest(new { Message = "Patient name is required." });
            }

            if (string.IsNullOrWhiteSpace(appointmentDto.PatientContactInfo))
            {
                return BadRequest(new { Message = "Patient contact information is required." });
            }

            if (appointmentDto.AppointmentDateTime <= DateTime.Now)
            {
                return BadRequest(new { Message = "Appointment date and time must be in the future." });
            }

            if (appointmentDto.DoctorId <= 0)
            {
                return BadRequest(new { Message = "Valid doctor ID is required." });
            }

            var appointment = new Appointment
            {
                PatientName = appointmentDto.PatientName,
                PatientContactInfo = appointmentDto.PatientContactInfo,
                AppointmentDateTime = appointmentDto.AppointmentDateTime,
                DoctorId = appointmentDto.DoctorId
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return Ok(new {Message= "Appointment successful" });
        }


        [HttpGet("appointments")]
        [Authorize]
        public IActionResult GetAppointments()
        {
            var appointments = _context.Appointments
                .Select(a => new AppointmentDetailsDTO
                {
                    AppointmentId = a.AppointmentId,
                    PatientName = a.PatientName,
                    PatientContactInfo = a.PatientContactInfo,
                    AppointmentDateTime = a.AppointmentDateTime,
                    doctorId=a.DoctorId,
                    DoctorName = a.Doctor.DoctorName
                })
                .ToList();

            return Ok(appointments);
        }


        [HttpGet("appointments/{id}")]
        [Authorize]
        public IActionResult GetAppointment(int id)
        {
            var appointment = _context.Appointments
       .Include(a => a.Doctor)
       .FirstOrDefault(a => a.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound(new { Message = "Appointment not found" });
            }

            var appointmentDto = new AppointmentDetailsDTO
            {
                AppointmentId = appointment.AppointmentId,
                PatientName = appointment.PatientName,
                PatientContactInfo = appointment.PatientContactInfo,
                AppointmentDateTime = appointment.AppointmentDateTime,
                doctorId=appointment.DoctorId,
                DoctorName = appointment?.Doctor?.DoctorName
            };

            return Ok(appointmentDto);
        }


        [HttpPut("appointments/{id}")]
        [Authorize]
        public IActionResult UpdateAppointment(int id, [FromBody] CreateUpdateAppointmentDTO appointmentDto)
        {
            var existingAppointment = _context.Appointments.Find(id);
            if (existingAppointment == null)
            {
                return NotFound(new { Message = "Appointment not found" });
            }

            existingAppointment.PatientName = appointmentDto.PatientName;
            existingAppointment.PatientContactInfo = appointmentDto.PatientContactInfo;
            existingAppointment.AppointmentDateTime = appointmentDto.AppointmentDateTime;
            existingAppointment.DoctorId = appointmentDto.DoctorId;

            _context.SaveChanges();

            
            var updatedAppointmentDto = new AppointmentDetailsDTO
            {
                AppointmentId = existingAppointment.AppointmentId,
                PatientName = existingAppointment.PatientName,
                PatientContactInfo = existingAppointment.PatientContactInfo,
                AppointmentDateTime = existingAppointment.AppointmentDateTime,
                DoctorName = existingAppointment?.Doctor?.DoctorName
            };

            return Ok(new {Message="Appointment Update Successful"});
        }



        [HttpDelete("appointments/{id}")]
        [Authorize]
        public IActionResult DeleteAppointment(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound(new { Message = "Appointment not found" });
            }

            _context.Appointments.Remove(appointment);
            _context.SaveChanges();

            return Ok(new { Message = "Appointment deleted successfully" });
        }


    }
}
