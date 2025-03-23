using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EMS_Project.Migrations
{
    /// <inheritdoc />
    public partial class Add_Admin_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "AdminId", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "rahul.sharma@example.com", "Rahul", "Sharma", "75K3eLr+dx6JJFuJ7LwIpEpOFmwGZZkRiB84PURz6U8=", "9876543210", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "priya.patel@example.com", "Priya", "Patel", "5+ycvz3BpCVipeUA1XaAAZM2JOqNjz6gYCCSxC1LyFc=", "9823456789", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "AdminId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "AdminId",
                keyValue: 2);
        }
    }
}
