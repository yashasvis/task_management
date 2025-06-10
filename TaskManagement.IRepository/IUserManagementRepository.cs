using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Models;

namespace TaskManagement.IRepository
{
    public interface IUserManagementRepository
    {
        Task<GetUsersWithPaginationModel> GetUsersWithPagination(int fetchRows, int skipRows, string? search, string? sortBy, string? direction);
        Task<int> SaveUser(Users user);
        Task<Users> GetUserById(int userId);
        Task<int> EditUser(Users user);
        Task<List<GetUsersForSelectModel>> GetUserList();
    }
}
