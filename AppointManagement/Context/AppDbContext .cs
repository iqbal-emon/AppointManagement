using AppointManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AppointManagement.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
