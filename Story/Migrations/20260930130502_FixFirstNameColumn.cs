using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixFirstNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1d80a19d-9f51-468d-a243-e0738556ae80");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4db4587b-82de-403c-8459-ff64a20f8436");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "94122702-a8c0-4025-b829-f88d55ee3cc2");

            migrationBuilder.RenameColumn(
                name: "FistName",
                table: "AspNetUsers",
                newName: "FirstName");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "13ea156d-31be-438a-a21f-5ce5e103420a", null, "Admin", "ADMIN" },
                    { "77c92f26-a373-40d7-8ca6-6ceeb71bb5af", null, "Editor", "EDITOR" },
                    { "9968bf52-8bea-4a55-9a64-40292440ad0c", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13ea156d-31be-438a-a21f-5ce5e103420a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "77c92f26-a373-40d7-8ca6-6ceeb71bb5af");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9968bf52-8bea-4a55-9a64-40292440ad0c");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "AspNetUsers",
                newName: "FistName");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1d80a19d-9f51-468d-a243-e0738556ae80", null, "User", "USER" },
                    { "4db4587b-82de-403c-8459-ff64a20f8436", null, "Editor", "EDITOR" },
                    { "94122702-a8c0-4025-b829-f88d55ee3cc2", null, "Admin", "ADMIN" }
                });
        }
    }
}
