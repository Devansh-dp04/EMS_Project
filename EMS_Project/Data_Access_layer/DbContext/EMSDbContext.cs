using Microsoft.EntityFrameworkCore;
using EMS_Project.Models;
using System.Text;
using System.Security.Cryptography;

namespace EMS_Project.Data_Access_layer.DbContext
{
    public class EMSDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public EMSDbContext(DbContextOptions<EMSDbContext> options) : base(options)
        { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>(entity =>
            {
                // Relationship with Department
                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Leave>(entity =>
            {
                entity.Property(l => l.EndDate)
                    .IsRequired();

                entity.Property(l => l.LeaveType)
                    .IsRequired()
                    .HasMaxLength(50);


                entity.Property(l => l.AppliedAt)
                    .HasDefaultValueSql("GETDATE()");

                

                entity.HasCheckConstraint("CK_Leave_LeaveType",
                    "LeaveType IN ('Sick', 'Casual', 'Vacation', 'Other')");

                // Check constraint for Status
                entity.HasCheckConstraint("CK_Leave_Status",
                    "Status IN ('Pending', 'Approved', 'Rejected')");

                // Relationship with Employee
                entity.HasOne(l => l.Employee)
                    .WithMany(e => e.Leaves)
                    .HasForeignKey(l => l.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Timesheet>(entity =>
           {

               entity.Property(t => t.TotalHours)
                   .IsRequired()
                   .HasColumnType("decimal(5,2)");

               entity.Property(t => t.StartTime)
                   .IsRequired();

               entity.Property(t => t.EndTime)
                   .IsRequired();

               entity.Property(t => t.TotalHours)
                   .IsRequired();

               entity.Property(t => t.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

               // Relationship with Employee
               entity.HasOne(t => t.Employee)
                   .WithMany(e => e.Timesheets)
                   .HasForeignKey(t => t.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);
           });
            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(d => d.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(d => d.DepartmentName)
                .IsRequired()
                .HasMaxLength(100);

                entity.HasMany(d => d.Employees)
                    .WithOne(e => e.Department)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    AdminId = 1,
                    FirstName = "Rahul",
                    LastName = "Sharma",
                    Email = "rahul.sharma@example.com",
                    Phone = "9876543210",
                    PasswordHash = GenerateHash("password123"),                    
                },
                new Admin
                {
                    AdminId = 2,
                    FirstName = "Priya",
                    LastName = "Patel",
                    Email = "priya.patel@example.com",
                    Phone = "9823456789",
                    PasswordHash = GenerateHash("admin@2025"),                    
                });

        }
        private string GenerateHash(string v)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(v));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}