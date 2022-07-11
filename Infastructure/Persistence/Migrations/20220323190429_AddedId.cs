using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infastructure.Migrations
{
    public partial class AddedId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Fridges_Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fridges_Products",
                table: "Fridges_Products",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Fridges_Products_FridgeId",
                table: "Fridges_Products",
                column: "FridgeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Fridges_Products",
                table: "Fridges_Products");

            migrationBuilder.DropIndex(
                name: "IX_Fridges_Products_FridgeId",
                table: "Fridges_Products");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "04afe6be-fdd0-422b-ac16-b93aa950fd91");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2fa30968-e023-4eb8-85e3-57ed86f0c9e3");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Fridges_Products");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Fridges");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Products",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fridges_Products",
                table: "Fridges_Products",
                columns: new[] { "FridgeId", "ProductId" });
        }
    }
}
