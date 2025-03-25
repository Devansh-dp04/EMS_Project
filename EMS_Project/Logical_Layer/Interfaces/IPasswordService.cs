namespace EMS_Project.Logical_Layer.Interfaces
{
    public interface IPasswordService
    {
        public string HashPassword(string password);

        //public string PasswordReset();
    }
}
