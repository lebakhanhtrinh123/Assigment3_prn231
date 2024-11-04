using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Fullname is required")]
        public string Fullname { get; set; } = null!;

        [Required(ErrorMessage = "BirthDay is required")]
        public DateTime BirthDay { get; set; } // Đổi từ DateOnly sang DateTime

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;
    }
}
