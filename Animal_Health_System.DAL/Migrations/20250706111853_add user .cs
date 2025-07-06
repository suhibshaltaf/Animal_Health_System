using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Animal_Health_System.DAL.Migrations
{
    /// <inheritdoc />
    public partial class adduser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47a40483-350c-4fbb-a37d-59cd65d9e9d5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d5c07b12-d93e-4191-87d4-85aa4b92a625", "168e07b3-77dd-485e-96ce-509abbe65afe" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3aa0d7db-7a3c-48d7-9ee6-9cb385c390a4", "853951fb-3305-4392-b9e4-37244c8bfa02" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "747f509e-c9ae-4bcf-bd79-3c16ae66e67c", "e417b8c6-4ea5-4f77-b5b9-94efca32912c" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6236ac17-870c-4a78-ba08-dc95fb29137d");

            migrationBuilder.DeleteData(
                table: "owners",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "veterinarians",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3aa0d7db-7a3c-48d7-9ee6-9cb385c390a4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "747f509e-c9ae-4bcf-bd79-3c16ae66e67c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d5c07b12-d93e-4191-87d4-85aa4b92a625");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "168e07b3-77dd-485e-96ce-509abbe65afe");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "853951fb-3305-4392-b9e4-37244c8bfa02");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e417b8c6-4ea5-4f77-b5b9-94efca32912c");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3aa0d7db-7a3c-48d7-9ee6-9cb385c390a4", null, "Veterinarian", "VETERINARIAN" },
                    { "47a40483-350c-4fbb-a37d-59cd65d9e9d5", null, "Admin", "ADMIN" },
                    { "747f509e-c9ae-4bcf-bd79-3c16ae66e67c", null, "Owner", "OWNER" },
                    { "d5c07b12-d93e-4191-87d4-85aa4b92a625", null, "FarmStaff", "FARMSTAFF" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "168e07b3-77dd-485e-96ce-509abbe65afe", 0, null, "0efb484f-de45-4ce2-b550-15be6ce78da0", "farmstaff@medc.com", true, null, false, null, "FARMSTAFF@MEDC.COM", "FARMSTAFF@MEDC.COM", "AQAAAAIAAYagAAAAEKnh19zDxucTDr0rv4ZWWGRv8Bh37QfLgmIA1gyZAd1AvnhfuLlLhKAJWVDdVVC2xA==", null, false, "FarmStaff", "51fcd890-0902-47e2-a3aa-311fa449452a", false, "farmstaff@medc.com" },
                    { "6236ac17-870c-4a78-ba08-dc95fb29137d", 0, null, "46ca33a5-bfc3-467b-b142-3bd47e456691", "admin@medc.com", true, null, false, null, "ADMIN@MEDC.COM", "ADMIN@MEDC.COM", "AQAAAAIAAYagAAAAEMFhPQyM9rq+9TpR9nHvUCsr1lqjbVMtYNPdVLBFISD8fkSv4+Bt3uWFnz+QyoURUA==", null, false, "Admin", "6b0dfd7f-33e8-489d-b060-ac691af80f32", false, "admin@medc.com" },
                    { "853951fb-3305-4392-b9e4-37244c8bfa02", 0, null, "e3d36b4c-52d3-4acc-9c5b-ecd82f581ec9", "veterinarian@medc.com", true, null, false, null, "VETERINARIAN@MEDC.COM", "VETERINARIAN@MEDC.COM", "AQAAAAIAAYagAAAAEOhCpUo7IroF+XMQOmV48vv918Y84qF2q/OpRAnInE9NUXidJnThEf6ZCAIggOjw3w==", null, false, "Veterinarian", "1aa4ab08-1b7c-4b2d-878c-807741cb7a7e", false, "veterinarian@medc.com" },
                    { "e417b8c6-4ea5-4f77-b5b9-94efca32912c", 0, null, "8e251ebf-a29a-4dca-8906-292adfdca4c0", "owner@medc.com", true, null, false, null, "OWNER@MEDC.COM", "OWNER@MEDC.COM", "AQAAAAIAAYagAAAAEKi/LQVxEFFJ66ym/UBJTKjywj0p/61+r+J3tLo/ynuAEiVx3n5FdpYbDm+/L5dlsA==", null, false, "Owner", "fae8410e-15c2-4c35-a46b-dffbdb480ce9", false, "owner@medc.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "d5c07b12-d93e-4191-87d4-85aa4b92a625", "168e07b3-77dd-485e-96ce-509abbe65afe" },
                    { "3aa0d7db-7a3c-48d7-9ee6-9cb385c390a4", "853951fb-3305-4392-b9e4-37244c8bfa02" },
                    { "747f509e-c9ae-4bcf-bd79-3c16ae66e67c", "e417b8c6-4ea5-4f77-b5b9-94efca32912c" }
                });

            migrationBuilder.InsertData(
                table: "owners",
                columns: new[] { "Id", "ApplicationUserId", "CreatedAt", "Email", "FullName", "IsDeleted", "PhoneNumber", "UpdatedAt" },
                values: new object[] { 1, "e417b8c6-4ea5-4f77-b5b9-94efca32912c", new DateTime(2025, 2, 25, 20, 14, 56, 220, DateTimeKind.Utc).AddTicks(2178), "owner@medc.com", "John Doe", false, "123-456-7890", new DateTime(2025, 2, 25, 20, 14, 56, 220, DateTimeKind.Utc).AddTicks(2184) });

            migrationBuilder.InsertData(
                table: "veterinarians",
                columns: new[] { "Id", "ApplicationUserId", "CreatedAt", "Email", "FullName", "IsDeleted", "PhoneNumber", "Specialty", "UpdatedAt", "YearsOfExperience", "salary" },
                values: new object[] { 1, "853951fb-3305-4392-b9e4-37244c8bfa02", new DateTime(2025, 2, 25, 20, 14, 56, 280, DateTimeKind.Utc).AddTicks(9384), "veterinarian@medc.com", "Dr. Smith", false, "123-456-7890", "Small Animal", new DateTime(2025, 2, 25, 20, 14, 56, 280, DateTimeKind.Utc).AddTicks(9390), 5, 50000m });
        }
    }
}
