using Microsoft.AspNetCore.Mvc;

namespace EMS_Project.Logical_Layer.Interfaces
{
    public interface ITimeSheetService
    {
        public Task<byte[]> ExportToExcelAsync();
    }
}
