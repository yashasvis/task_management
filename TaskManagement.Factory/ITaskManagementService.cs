using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Models;

namespace TaskManagement.Factory
{
    public interface ITaskManagementService
    {
        Task<GetTaskWithPaginationModel> GetTaskWithPagination(int fetchRows, int skipRows, string? search, string? sortBy, string? direction);
        Task<int> SaveTask(Tasks task);
        Task<Tasks> GetTaskById(int Id);
        Task<int> UpdateTask(Tasks task);
    }
}
