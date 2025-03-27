namespace EMS_Project.Logical_Layer.Interfaces
{
    public interface IJWTService
    {
        public string GenerateJWTToken(string email, string password, string role, int empid);
    }
}
