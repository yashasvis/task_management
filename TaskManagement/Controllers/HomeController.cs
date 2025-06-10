using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System.Diagnostics;
using TaskManagement.Factory;
using TaskManagement.Models;
using TaskManagement.Service;

namespace TaskManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITaskManagementService _taskManagementService;
        private readonly IUserManagementService _userManagementService;
        private readonly IMapper _mapper;
        public HomeController(ITaskManagementService taskManagementService,
            IUserManagementService userManagementService,
             IMapper mapper)
        {
            _taskManagementService = taskManagementService;
            _userManagementService = userManagementService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string? message)
        {
            MainTaskModel model = new MainTaskModel();
            model.message = message;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetTaskWithPagination()
        {
            try
            {
                GetTaskWithPaginationModel model = new GetTaskWithPaginationModel();
                int totalRecord = 0;
                int filterRecord = 0;
                var draw = Request.Form["draw"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
                model = await _taskManagementService.GetTaskWithPagination(pageSize, skip, searchValue!, sortColumn!, sortColumnDirection!);
                filterRecord = model.totalRecords;
                totalRecord = model.totalRecords;
                var returnObj = new
                {
                    draw = draw,
                    recordsTotal = totalRecord,
                    recordsFiltered = filterRecord,
                    data = model.tasks
                };
                return new JsonResult(returnObj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("create-task")]
        public async Task<IActionResult> Create()
        {
            var users = await _userManagementService.GetUserList();
            List<SelectListItem> assigneeList = new List<SelectListItem>();
            List<SelectListItem> reviewerList = new List<SelectListItem>();
            List<SelectListItem> statusList = new List<SelectListItem>();
            List<SelectListItem> typeList = new List<SelectListItem>();
            assigneeList.Add(new SelectListItem()
            {
                Text = "Task Assigned To",
                Value = ""
            });
            reviewerList.Add(new SelectListItem()
            {
                Text = "Task Reviewer",
                Value = ""
            });
            foreach (var user in users)
            {
                assigneeList.Add(new SelectListItem()
                {
                    Text = user.Name,
                    Value = user.Id.ToString()
                });

                reviewerList.Add(new SelectListItem()
                {
                    Text = user.Name,
                    Value = user.Id.ToString()
                });
            }

            statusList = GetTaskStatus();
            typeList = GetTaskType();
            TasksDTO task = new TasksDTO();
            task.ReviewerList = reviewerList;
            task.AssigneeList = assigneeList;
            task.statusList = statusList;
            task.typeList = typeList;
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Creates(TasksDTO task)
        {
            var users = await _userManagementService.GetUserList();
            List<SelectListItem> assigneeList = new List<SelectListItem>();
            List<SelectListItem> reviewerList = new List<SelectListItem>();
            List<SelectListItem> statusList = new List<SelectListItem>();
            List<SelectListItem> typeList = new List<SelectListItem>();
            assigneeList.Add(new SelectListItem()
            {
                Text = "Task Assigned To",
                Value = ""
            });
            reviewerList.Add(new SelectListItem()
            {
                Text = "Task Reviewer",
                Value = ""
            });
            foreach (var user in users)
            {
                assigneeList.Add(new SelectListItem()
                {
                    Text = user.Name,
                    Value = user.Id.ToString()
                });

                reviewerList.Add(new SelectListItem()
                {
                    Text = user.Name,
                    Value = user.Id.ToString()
                });
            }

            statusList = GetTaskStatus();
            typeList = GetTaskType();
            task.ReviewerList = reviewerList;
            task.AssigneeList = assigneeList;
            task.statusList = statusList;
            task.typeList = typeList;

            if (ModelState.IsValid)
            {
                int taskId = 0;
                string message = "";
                if (ModelState.IsValid)
                {
                    var taskDTO = _mapper.Map<Tasks>(task);
                    taskId = await _taskManagementService.SaveTask(taskDTO);
                    if (taskId == 0)
                        message = "Something went wrong while saving task, please try again.!";
                    else
                        message = "New Task added successfully.!";
                    return RedirectToAction("Index", "Home", new { message = message });
                }
            }

            return View("Create", task);
        }

        [HttpGet]
        [Route("edit-task")]
        public async Task<IActionResult> Edit(int id)
        {
            var users = await _userManagementService.GetUserList();
            List<SelectListItem> assigneeList = new List<SelectListItem>();
            List<SelectListItem> reviewerList = new List<SelectListItem>();
            List<SelectListItem> statusList = new List<SelectListItem>();
            List<SelectListItem> typeList = new List<SelectListItem>();
            assigneeList.Add(new SelectListItem()
            {
                Text = "Task Assigned To",
                Value = ""
            });
            reviewerList.Add(new SelectListItem()
            {
                Text = "Task Reviewer",
                Value = ""
            });
            foreach (var user in users)
            {
                assigneeList.Add(new SelectListItem()
                {
                    Text = user.Name,
                    Value = user.Id.ToString()
                });

                reviewerList.Add(new SelectListItem()
                {
                    Text = user.Name,
                    Value = user.Id.ToString()
                });
            }

            statusList = GetTaskStatus();
            typeList = GetTaskType();
            Tasks task = new Tasks();
            task = await _taskManagementService.GetTaskById(id);
            var taskDTO = _mapper.Map<TasksDTO>(task);
            taskDTO.ReviewerList = reviewerList;
            taskDTO.AssigneeList = assigneeList;
            taskDTO.statusList = statusList;
            taskDTO.typeList = typeList;
            taskDTO.DueDates = task.DueDate != null ? Convert.ToDateTime(task.DueDate).ToString("yyyy-MM-dd") : null;
            return View(taskDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Edits(TasksDTO task)
        {
            var users = await _userManagementService.GetUserList();
            List<SelectListItem> assigneeList = new List<SelectListItem>();
            List<SelectListItem> reviewerList = new List<SelectListItem>();
            List<SelectListItem> statusList = new List<SelectListItem>();
            List<SelectListItem> typeList = new List<SelectListItem>();
            assigneeList.Add(new SelectListItem()
            {
                Text = "Task Assigned To",
                Value = ""
            });
            reviewerList.Add(new SelectListItem()
            {
                Text = "Task Reviewer",
                Value = ""
            });
            foreach (var user in users)
            {
                assigneeList.Add(new SelectListItem()
                {
                    Text = user.Name,
                    Value = user.Id.ToString()
                });

                reviewerList.Add(new SelectListItem()
                {
                    Text = user.Name,
                    Value = user.Id.ToString()
                });
            }

            statusList = GetTaskStatus();
            typeList = GetTaskType();
            task.ReviewerList = reviewerList;
            task.AssigneeList = assigneeList;
            task.statusList = statusList;
            task.typeList = typeList;

            if (ModelState.IsValid)
            {
                int taskId = 0;
                string message = "";
                if (ModelState.IsValid)
                {
                    var taskDTO = _mapper.Map<Tasks>(task);
                    taskId = await _taskManagementService.UpdateTask(taskDTO);
                    if (taskId == 0)
                        message = "Something went wrong while saving task, please try again.!";
                    else
                        message = "Task's details updated successfully.!";
                    return RedirectToAction("Index", "Home", new { message = message });
                }
            }

            return View("Create", task);
        }

        public List<SelectListItem> GetTaskStatus()
        {
            List<SelectListItem> statusList = new List<SelectListItem>();

            statusList.Add(new SelectListItem()
            {
                Text = "New",
                Value = "New"
            });

            statusList.Add(new SelectListItem()
            {
                Text = "In Progress",
                Value = "In Progress"
            });

            statusList.Add(new SelectListItem()
            {
                Text = "Completed",
                Value = "Completed"
            });

            return statusList;
        }

        public List<SelectListItem> GetTaskType()
        {
            List<SelectListItem> typeList = new List<SelectListItem>();

            typeList.Add(new SelectListItem()
            {
                Text = "Milestone",
                Value = "Milestone"
            });

            typeList.Add(new SelectListItem()
            {
                Text = "Compliance",
                Value = "Compliance"
            });

            typeList.Add(new SelectListItem()
            {
                Text = "Task",
                Value = "Task"
            });

            return typeList;
        }
    }
}
