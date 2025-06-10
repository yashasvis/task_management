using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Users, UsersDTO>();
            CreateMap<UsersDTO, Users>();

            CreateMap<Tasks, TasksDTO>();
            CreateMap<TasksDTO, Tasks>();
        }
        
    }
}
