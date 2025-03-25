using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EMS_Project.Models;

namespace EMS_Project.Logical_Layer.DTOs
{
    public class UpdateEmployeeByEmployeeDTO
    {
        
        [StringLength(15)]
        public string Phone { get; set; }
        public string Address { get; set; }

        public string TechStack { get; set; }
    }
    public class MappingProfiler : Profile
    {
        public MappingProfiler()
        {
            CreateMap<Employee, UpdateEmployeeByEmployeeDTO>();

            CreateMap<UpdateEmployeeByEmployeeDTO, Employee>();
                
        }
    }
}
