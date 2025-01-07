﻿using System.ComponentModel.DataAnnotations;

namespace AppointManagement.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
    }

}
