using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EMS_Project.Models;

namespace EMS_Project.Logical_Layer.DTOs
{
    public class UpdateEmployeeDTO
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[bB][aA][cC][aA][nN][cC][yY]\.com$",
            ErrorMessage = "Email must be in the format abc.xyz@bacancy.com")]
        public string Email { get; set; }

        [Required]
        public int DepartmentId { get; set; }   

        public DateTime? DateOfBirth { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }

    public class MappingProfile : Profile {
        public MappingProfile()
        {
            CreateMap<Employee, UpdateEmployeeDTO>();
            CreateMap<UpdateEmployeeDTO, Employee>()
                .ForMember(dest => dest.EmployeeId, opt => opt.Ignore())          
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))  
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow)); 
        }
    }
}
