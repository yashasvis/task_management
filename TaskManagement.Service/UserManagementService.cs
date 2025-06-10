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
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserManagementRepository _userManagementRepository;
        public UserManagementService(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }
        public async Task<GetUsersWithPaginationModel> GetUsersWithPagination(int fetchRows, int skipRows, string? search, string? sortBy, string? direction)
        {
            return await _userManagementRepository.GetUsersWithPagination(fetchRows, skipRows, search, sortBy, direction);
        }
        public async Task<int> SaveUser(Users user)
        {
            return await _userManagementRepository.SaveUser(user);
        }
        public async Task<Users> GetUserById(int userId)
        {
            return await _userManagementRepository.GetUserById(userId);
        }
        public async Task<int> EditUser(Users user)
        {
            return await _userManagementRepository.EditUser(user);
        }
        public async Task<List<GetUsersForSelectModel>> GetUserList()
        {
            return await _userManagementRepository.GetUserList();
        }
    }
}
