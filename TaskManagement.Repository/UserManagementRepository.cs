using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.DAL;
using TaskManagement.IRepository;
using TaskManagement.Models;
using System.Linq.Dynamic.Core;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace TaskManagement.Repository
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly TaskManagementDBContext _context;
        public UserManagementRepository(TaskManagementDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Users With Pagination 
        /// </summary>
        /// <param name="fetchRows"></param>
        /// <param name="skipRows"></param>
        /// <param name="search"></param>
        /// <param name="sortBy"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<GetUsersWithPaginationModel> GetUsersWithPagination(int fetchRows,int skipRows,string? search,string? sortBy,string? direction)
        {
            try
            {
                GetUsersWithPaginationModel model = new GetUsersWithPaginationModel();
                List<GetUsersModel> users = new List<GetUsersModel>();
                int totalRecords = 0,index=0;
                if (skipRows > 0)
                    index = skipRows + 1;
                else
                    index = 1;

                if (direction == null || direction == "")
                    direction = "asc";

                if (sortBy == null || sortBy == "" || sortBy == "#")
                    sortBy = "Id";
                else if (sortBy == "NAME")
                    sortBy = "Firstname";

                string sorting = $"{sortBy} {direction}";

                if (search != null && search != "")
                {
                    totalRecords = await _context.Users.Where((x) => (x.Firstname + " " + x.Lastname) == search).CountAsync();
                       
                    var getUsers = await _context.Users
                        .Where((x) => (x.Firstname + " " + x.Lastname) == search)
                        .OrderBy(sorting)
                        .Skip(skipRows).Take(fetchRows).ToListAsync();

                    foreach(var item in getUsers)
                    {
                        string name = item.Firstname;
                        if (item.Lastname != "" && item.Lastname != null)
                            name = $"{name} {item.Lastname}";
                        users.Add(new GetUsersModel
                        {
                            index = index,
                            Id = item.Id,
                            Name = name,
                        });
                        index++;
                    }
                }
                else
                {
                    totalRecords = await _context.Users.CountAsync();

                    var getUsers = await _context.Users
                        .OrderBy(sorting)
                        .Skip(skipRows).Take(fetchRows).ToListAsync();

                    foreach (var item in getUsers)
                    {
                        string name = item.Firstname;
                        if (item.Lastname != "" && item.Lastname != null)
                            name = $"{name} {item.Lastname}";
                        users.Add(new GetUsersModel 
                        {
                            index = index,
                            Id = item.Id,
                            Name = name,
                        });
                        index++;
                    }
                }
                
                model.totalRecords = totalRecords;
                model.users = users;    
                return model;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Save User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> SaveUser(Users user)
        {
            int userId = 0;
            try
            {
                
                user.CreatedOnUtc = DateTime.Now;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                userId = 1;
                return userId;
            }
            catch (Exception ex)
            {
                userId = 0;
                return userId;
            }
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Users> GetUserById(int userId)
        {
            try
            {
                Users model = new Users();
                model = await _context.Users.Where((x) => x.Id == userId).FirstAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Edit user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> EditUser(Users user)
        {
            int userId = 0;
            try
            {
                user.ModifyOnUtc = DateTime.Now;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                userId = 1;
                return userId;
            }
            catch (Exception ex)
            {
                userId = 0;
                return userId;
            }
        }

        /// <summary>
        /// Get all user list
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<GetUsersForSelectModel>> GetUserList()
        {
            try
            {
                List<GetUsersForSelectModel> model = new List<GetUsersForSelectModel>();
                var user = await _context.Users.ToListAsync();
                foreach (var item in user)
                {
                    string name = item.Firstname;
                    if (item.Lastname != "" && item.Lastname != null)
                        name = $"{name} {item.Lastname}";
                    model.Add(new GetUsersForSelectModel
                    {
                        Id = item.Id,
                        Name = name,
                    });
                }
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
