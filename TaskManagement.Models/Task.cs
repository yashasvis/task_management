using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Models
{
    public class Tasks
    {
        public int Id {  get; set; }

        [DisplayName("TASK TITLE")]
        [Required(ErrorMessage = "Please provide title for task.")]
        [MaxLength(250,ErrorMessage = "Title exceed maximum length , Task title should be less than 250 characters.")]
        public string Title { get; set; }

        [DisplayName("TASK DESCRIPTION")]
        [Required(ErrorMessage = "Please provide description for task.")]
        public string Description { get; set; }

        [DisplayName("DUE DATE")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        [DisplayName("TASK ASSIGNED TO")]
        [Required(ErrorMessage = "Please select whom you want to assign this task.")]
        public int Assignee {  get; set; }

        [DisplayName("TASK REVIEWER")]
        public int Reviewer {  get; set; }

        [DisplayName("STATUS")]
        [Required(ErrorMessage = "Please select status of task.")]
        public string Status {  get; set; }

        [DisplayName("TASK TYPE")]
        [Required(ErrorMessage = "Please select type of task.")]
        public string Type { get; set; }
        public DateTime? CreatedOnUtc {  get; set; }
        public DateTime? ModifyOnUtc { get; set; }
    }
    public class GetTasksModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DueDate { get; set; }
        public string Assignee { get; set; }    
        public string Reviewer { get; set; }    
        public string Status { get; set; }
        public string Type { get; set; }
    }

    public class GetTaskWithPaginationModel
    {
        public GetTaskWithPaginationModel()
        {
            tasks = new List<GetTasksModel>();
        }
        public List<GetTasksModel> tasks {  get; set; }
        public int totalRecords { get; set; }
    }

    public class MainTaskModel
    {
        public string? message { get; set; }
    }
}
