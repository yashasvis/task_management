using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Models;

namespace TaskManagement.DAL
{
    public class TaskManagementDBContext : DbContext
    {
        public TaskManagementDBContext(DbContextOptions<TaskManagementDBContext> options) : base(options) { }

        public DbSet<Tasks> Tasks { get; set; }
    }
}
