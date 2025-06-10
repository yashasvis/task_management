using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Factory;
using TaskManagement.IRepository;
using TaskManagement.Models;

namespace TaskManagement.Service
{
    public class TaskManagementService : ITaskManagementService
    {
        private readonly ITaskManagementRepository _taskManagementRepository;
        public TaskManagementService(ITaskManagementRepository taskManagementRepository)
        {
            _taskManagementRepository = taskManagementRepository;
        }
        public async Task<GetTaskWithPaginationModel> GetTaskWithPagination(int fetchRows, int skipRows, string? search, string? sortBy, string? direction)
        {
            return await _taskManagementRepository.GetTaskWithPagination(fetchRows, skipRows, search, sortBy, direction);
        }
        public async Task<int> SaveTask(Tasks task)
        {
            return await _taskManagementRepository.SaveTask(task);
        }
        public async Task<Tasks> GetTaskById(int Id)
        {
            return await _taskManagementRepository.GetTaskById(Id);
        }
        public async Task<int> UpdateTask(Tasks task)
        {
            return await _taskManagementRepository.UpdateTask(task);
        }
    }
}
