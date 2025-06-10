using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Models
{
    public class UsersDTO
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "Please provide first name of user.")]
        [MaxLength(100, ErrorMessage = "User's first name exceed maximum length , First name should be less than 100 character.")]
        public string Firstname { get; set; }

        [DisplayName("Last Name")]
        public string? Lastname { get; set; }
    }
}
