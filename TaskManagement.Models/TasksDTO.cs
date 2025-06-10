using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagement.Models
{
    public class TasksDTO
    {
        public TasksDTO()
        {
            AssigneeList = new List<SelectListItem>();
            ReviewerList = new List<SelectListItem>();
            statusList = new List<SelectListItem>();
            typeList = new List<SelectListItem>();
        }
        public int Id { get; set; }

        [DisplayName("TASK TITLE")]
        [Required(ErrorMessage = "Please provide title for task.")]
        [MaxLength(250, ErrorMessage = "Title exceed maximum length , Task title should be less than 250 characters.")]
        public string Title { get; set; }

        [DisplayName("TASK DESCRIPTION")]
        [Required(ErrorMessage = "Please provide description for task.")]
        public string Description { get; set; }

        [DisplayName("DUE DATE")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }
        public string? DueDates { get; set; }

        [DisplayName("TASK ASSIGNED TO")]
        [Required(ErrorMessage = "Please select whom you want to assign this task.")]
        public int Assignee { get; set; }

        [DisplayName("TASK REVIEWER")]
        public int? Reviewer { get; set; }

        [DisplayName("STATUS")]
        [Required(ErrorMessage = "Please select status of task.")]
        public string Status { get; set; }

        [DisplayName("TASK TYPE")]
        [Required(ErrorMessage = "Please select type of task.")]
        public string Type { get; set; }

        [DisplayName("TASK ASSIGNED TO")]
        [Required(ErrorMessage = "Please select whom you want to assign this task.")]
        public List<SelectListItem> AssigneeList { get; set; }

        [DisplayName("TASK REVIEWER")]
        public List<SelectListItem> ReviewerList { get; set; }

        [DisplayName("STATUS")]
        public List<SelectListItem> statusList { get; set; }

        [DisplayName("TASK TYPE")]
        public List<SelectListItem> typeList { get; set; }
    }
}
