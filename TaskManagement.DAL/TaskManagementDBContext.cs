using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagement.Models;

namespace TaskManagement.DAL
{
    public class TaskManagementDBContext : DbContext
    {
        public TaskManagementDBContext(DbContextOptions<TaskManagementDBContext> options) : base(options) { }

        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
