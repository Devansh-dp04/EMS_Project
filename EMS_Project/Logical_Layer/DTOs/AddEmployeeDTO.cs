using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Spreadsheet;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using AutoMapper;
using EMS_Project.Models;
using EMS_Project.Logical_Layer.DTOs;

namespace EMS_Project.Logical_Layer.DTOs
{
    public class AddEmployeeDTO
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(255)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[bB][aA][cC][aA][nN][cC][yY]\.com$",
        ErrorMessage = "Email must be in the format abc.xyz@bacancy.com")]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string Phone { get; set; }

        public string Password { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Address { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string TechStack { get; set; }

        public bool IsActive { get; set; } = true;
    }
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddEmployeeDTO, Employee>()
            .ForMember(dest => dest.EmployeeId, opt => opt.Ignore())
            .ForMember(dest => dest.Leaves, opt => opt.Ignore()).
            ForMember(dest => dest.PasswordHash, opt => opt.Ignore()).
            ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)).
            ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

         CreateMap<AddEmployeeDTO, Department >().
            ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.DepartmentName));

    }
}
