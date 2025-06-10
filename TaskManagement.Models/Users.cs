using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Models
{
    public class MainUserModel
    {
        public string? message { get; set; }
    }
    public class Users
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "Please provide first name of user.")]
        [MaxLength(100, ErrorMessage = "User's first name exceed maximum length , First name should be less than 100 character.")]
        public string Firstname { get; set; }

        [DisplayName("Last Name")]
        public string? Lastname { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public DateTime? ModifyOnUtc { get; set; }
    }

    public class GetUsersModel
    {
        public int index {  get; set; }
        public int Id {  get; set; }
        public string Name { get; set; }
    }

    public class GetUsersForSelectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class GetUsersWithPaginationModel
    {
        public GetUsersWithPaginationModel()
        {
            users = new List<GetUsersModel>();
        }
        public List<GetUsersModel> users {  get; set; }
        public int totalRecords {  get; set; }
    }
}
