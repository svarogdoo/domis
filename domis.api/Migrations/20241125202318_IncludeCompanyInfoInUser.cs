using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace domis.api.Migrations
{
    /// <inheritdoc />
    public partial class IncludeCompanyInfoInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "CompanyInfoId",
                schema: "identity",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompanyInfo",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Number = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyInfoId",
                schema: "identity",
                table: "AspNetUsers",
                column: "CompanyInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CompanyInfo_CompanyInfoId",
                schema: "identity",
                table: "AspNetUsers",
                column: "CompanyInfoId",
                principalSchema: "identity",
                principalTable: "CompanyInfo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CompanyInfo_CompanyInfoId",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CompanyInfo",
                schema: "identity");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyInfoId",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyInfoId",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
