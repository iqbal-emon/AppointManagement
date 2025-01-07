using AppointManagement.Context;
using AppointManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult CreateAppointment([FromBody] Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            _context.SaveChanges();
            return Ok(appointment);
        }

        [HttpGet("appointments")]
        [Authorize]
        public IActionResult GetAppointments()
        {
            return Ok(_context.Appointments);
        }
        [HttpGet("appointments/{id}")]
        [Authorize]
        public IActionResult GetAppointment(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }
        [HttpPut("appointments/{id}")]
        [Authorize]
        public IActionResult UpdateAppointment(int id, [FromBody] Appointment appointment)
        {
            var existingAppointment = _context.Appointments.Find(id);
            if (existingAppointment == null)
            {
                return NotFound();
            }
            existingAppointment.PatientName = appointment.PatientName;
            existingAppointment.PatientContactInfo = appointment.PatientContactInfo;
            existingAppointment.AppointmentDateTime = appointment.AppointmentDateTime;
            _context.SaveChanges();
            return Ok(existingAppointment);
        }
        [HttpDelete("appointments/{id}")]
        [Authorize]
        public IActionResult DeleteAppointment(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }
            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
            return Ok();
        }

    }
}
