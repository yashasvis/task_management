using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.DAL;
using TaskManagement.IRepository;
using TaskManagement.Models;

namespace TaskManagement.Repository
{
    public class TaskManagementRepository : ITaskManagementRepository 
    {
        private readonly TaskManagementDBContext _context;
        public TaskManagementRepository(TaskManagementDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get task with pagination.
        /// </summary>
        /// <param name="fetchRows"></param>
        /// <param name="skipRows"></param>
        /// <param name="search"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<GetTaskWithPaginationModel> GetTaskWithPagination(int fetchRows, int skipRows, string? search, string? sortBy, string? direction)
        {
            try
            {
                GetTaskWithPaginationModel model = new GetTaskWithPaginationModel();
                List<GetTasksModel> tasks = new List<GetTasksModel>();

                int totalRecords = 0, index = 0;
                if (skipRows > 0)
                    index = skipRows + 1;
                else
                    index = 1;

                if (direction == null || direction == "")
                    direction = "asc";

                if (sortBy == null || sortBy == "" || sortBy == "#")
                    sortBy = "Type";
                else if(sortBy == "DUE DATE")
                    sortBy = "Id";

                string sorting = $"{sortBy} {direction}";

                if (search != null && search != "")
                {
                    totalRecords = await _context.Tasks
                                .Join(
                                    _context.Users,
                                    task => task.Assignee,
                                    user => user.Id,
                                    (task, assignee) => new { task, assignee }
                                )
                                .GroupJoin(
                                    _context.Users,
                                    combined => combined.task.Reviewer,
                                    reviewer => reviewer.Id,
                                    (combined, reviewers) => new { combined.task, combined.assignee, reviewers }
                                )
                                .SelectMany(
                                    x => x.reviewers.DefaultIfEmpty(),
                                    (combined, reviewer) => new GetTasksModel
                                    {
                                        Id = combined.task.Id,
                                        Description = combined.task.Description,
                                        Title = combined.task.Title,
                                        DueDate = combined.task.DueDate.HasValue ? combined.task.DueDate.Value.ToString("dd MMM,yyyy") : "",
                                        Assignee = combined.assignee.Firstname + " " + combined.assignee.Lastname,
                                        Reviewer = reviewer != null ? reviewer.Firstname + " " + reviewer.Lastname : "N/A",
                                        Status = combined.task.Status,
                                        Type = combined.task.Type,
                                    }
                                )
                                .Where((x) =>
                                    x.Title.Contains(search) ||
                                    x.Description.Contains(search) ||
                                    x.Assignee.Contains(search) ||
                                    x.Reviewer.Contains(search) ||
                                    x.Status.Contains(search) ||
                                    x.Type.Contains(search) 
                                )
                                .CountAsync();

                    tasks = await _context.Tasks
                                .Join(
                                    _context.Users,
                                    task => task.Assignee,
                                    user => user.Id,
                                    (task, assignee) => new { task, assignee }
                                )
                                .GroupJoin(
                                    _context.Users,
                                    combined => combined.task.Reviewer,
                                    reviewer => reviewer.Id,
                                    (combined, reviewers) => new { combined.task, combined.assignee, reviewers }
                                )
                                .SelectMany(
                                    x => x.reviewers.DefaultIfEmpty(),
                                    (combined, reviewer) => new GetTasksModel
                                    {
                                        Id = combined.task.Id,
                                        Description = combined.task.Description,
                                        Title = combined.task.Title,
                                        DueDate = combined.task.DueDate.HasValue ? combined.task.DueDate.Value.ToString("dd MMM,yyyy") : "",
                                        Assignee = combined.assignee.Firstname + " " + combined.assignee.Lastname,
                                        Reviewer = reviewer != null ? reviewer.Firstname + " " + reviewer.Lastname : "N/A",
                                        Status = combined.task.Status,
                                        Type = combined.task.Type,
                                    }
                                )
                                .Where((x) =>
                                    x.Title.Contains(search) ||
                                    x.Description.Contains(search) ||
                                    x.Assignee.Contains(search) ||
                                    x.Reviewer.Contains(search) ||
                                    x.Status.Contains(search) ||
                                    x.Type.Contains(search)
                                )
                                .OrderBy(sorting)
                                .Skip(skipRows)
                                .Take(fetchRows)
                                .ToListAsync();
                }
                else
                {
                    totalRecords = await _context.Tasks.CountAsync();

                    tasks = await _context.Tasks
                                .Join(
                                    _context.Users,
                                    task => task.Assignee,
                                    user => user.Id,
                                    (task, assignee) => new { task, assignee }
                                )
                                .GroupJoin( 
                                    _context.Users,
                                    combined => combined.task.Reviewer,
                                    reviewer => reviewer.Id,
                                    (combined, reviewers) => new { combined.task, combined.assignee, reviewers }
                                )
                                .SelectMany( 
                                    x => x.reviewers.DefaultIfEmpty(), 
                                    (combined, reviewer) => new GetTasksModel
                                    {
                                        Id = combined.task.Id,
                                        Description = combined.task.Description,
                                        Title = combined.task.Title,
                                        DueDate = combined.task.DueDate.HasValue ? combined.task.DueDate.Value.ToString("dd MMM,yyyy") : "",
                                        Assignee = combined.assignee.Firstname + " " + combined.assignee.Lastname,
                                        Reviewer = reviewer != null ? reviewer.Firstname + " " + reviewer.Lastname : "N/A", 
                                        Status = combined.task.Status,
                                        Type = combined.task.Type,
                                    }
                                )
                                .OrderBy(sorting)
                                .Skip(skipRows)
                                .Take(fetchRows)
                                .ToListAsync();

                }
                model.totalRecords = totalRecords;
                model.tasks = tasks;
                return model;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Save Task
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> SaveTask(Tasks task)
        {
            int taskId = 0;
            try
            {
                Tasks saveTask = new Tasks
                {
                    Id= task.Id,    
                    Title = task.Title,
                    Description = task.Description,
                    Assignee = task.Assignee,
                    Reviewer = task.Reviewer,
                    Status = task.Status,
                    Type = task.Type,
                    DueDate = task.DueDate,
                    CreatedOnUtc = DateTime.Now
                };
                _context.Tasks.Add(saveTask);
                await _context.SaveChangesAsync();
                taskId = 1;
                return taskId;
            }
            catch (Exception ex)
            {
                taskId = 0;
                return taskId;
            }
        }

        /// <summary>
        /// Get task details by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Tasks> GetTaskById(int Id)
        {
            try
            {
                var task = await _context.Tasks.Where((x) => x.Id == Id).FirstAsync();
                Tasks taskDetail = new Tasks
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Assignee = task.Assignee,
                    Reviewer = task.Reviewer,
                    Status = task.Status,
                    Type = task.Type,
                    DueDate = task.DueDate != null ? Convert.ToDateTime(Convert.ToDateTime(task.DueDate).ToString("yyyy-MM-dd")) : null,
                };
                return taskDetail;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Update Task
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> UpdateTask(Tasks task)
        {
            int taskId = 0;
            try
            {
                Tasks saveTask = new Tasks
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Assignee = task.Assignee,
                    Reviewer = task.Reviewer,
                    Status = task.Status,
                    Type = task.Type,
                    DueDate = task.DueDate,
                    ModifyOnUtc = DateTime.Now
                };
                _context.Tasks.Update(saveTask);
                await _context.SaveChangesAsync();
                taskId = 1;
                return taskId;
            }
            catch (Exception ex)
            {
                taskId = 0;
                return taskId;
            }
        }

    }
}
