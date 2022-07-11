using Microsoft.EntityFrameworkCore.Migrations;

namespace Infastructure.Migrations
{
    public partial class AddedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "00f2cadf-f0e9-4dda-a4b6-a391b22eec16", "8b5c74c8-ef4c-44bf-9e3e-f8bcf2eac275", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "75a6d9ae-5f6c-4d6f-904c-0c34580c317a", "92adeefa-c3b4-427d-910f-ea2d621891b3", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "00f2cadf-f0e9-4dda-a4b6-a391b22eec16");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "75a6d9ae-5f6c-4d6f-904c-0c34580c317a");
        }
    }
}
