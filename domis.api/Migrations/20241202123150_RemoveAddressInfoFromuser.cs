using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace domis.api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAddressInfoFromuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Apartment",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "City",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Country",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "County",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "identity",
                table: "CompanyInfo",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "identity",
                table: "CompanyInfo",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "identity",
                table: "CompanyInfo");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "identity",
                table: "CompanyInfo");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Apartment",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "County",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
