using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace domis.api.Migrations
{
    /// <inheritdoc />
    public partial class AddDateOfBirthToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "identity",
                table: "AspNetUsers");
        }
    }
}
