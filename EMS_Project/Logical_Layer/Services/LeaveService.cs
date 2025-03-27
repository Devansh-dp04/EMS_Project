using AutoMapper;
using EMS_Project.Logical_Layer.DTOs;
using EMS_Project.Logical_Layer.Interfaces;
using EMS_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EMS_Project.Logical_Layer.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IMapper _mapper;

        public LeaveService(ILeaveRepository leaveRepository, IMapper mapper)
        {
            _leaveRepository = leaveRepository;
            _mapper = mapper;
        }       

        public async Task<LeaveDTO> GetLeaveByIdAsync(int id)
        {
            var leave = await _leaveRepository.GetLeaveByIdAsync(id);
            return _mapper.Map<LeaveDTO>(leave);
        }
        public async Task<bool> HasLeaveOnDateAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            return await _leaveRepository.HasLeaveOnDateAsync(employeeId, startDate, endDate);
        }
        public async Task AddLeaveAsync(LeaveDTO leaveDTO, int empid)
        {
            
            bool alreadyApplied = await _leaveRepository.HasLeaveOnDateAsync(empid, leaveDTO.StartDate, leaveDTO.EndDate);

            if (alreadyApplied)
            {
                throw new InvalidOperationException("You have already applied for leave on this date.");
            }

            var leave = new Leave
            {
                EmployeeId =empid,
                StartDate = leaveDTO.StartDate,
                EndDate = leaveDTO.EndDate,
                LeaveType = leaveDTO.LeaveType,
                Reason = leaveDTO.Reason,
                Status = "Pending",
                AppliedAt = DateTime.Now
            };

            await _leaveRepository.AddLeaveAsync(leave);
        }

        public async Task<IActionResult> GetLeavesByEmployeeIdAsync(int employeeId)
        {
            var leaves = await _leaveRepository.GetLeavesByEmployeeIdAsync(employeeId);
            if (leaves.IsNullOrEmpty())
            {
                return new ObjectResult(new
                {
                    Success = false,
                    
                    Message = "Applied for NoLeaves"
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            return new ObjectResult(new
            {
                Success = true,
                Data = leaves,
                Message = "Data retrieved"
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }      

        public async Task<IActionResult> UpdateLeaveStatusAsync(int empid, string leavestatus)
        {
            var leave = await _leaveRepository.UpdateLeaveStatusAsync(leavestatus,empid);
            if (leave == false) {
                return new ObjectResult(new
                {
                    Success = false,
                    Message = "Employee id is false"
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            } ;
            return new ObjectResult(new
            {
                Success = true,
                Message = "Status Updated Successfully"
            })
            {
                StatusCode = StatusCodes.Status200OK
            }; ;
        }

        public async Task<bool> DeleteLeaveAsync(int id)
        {
            return await _leaveRepository.DeleteLeaveAsync(id);
        }
    }
}
