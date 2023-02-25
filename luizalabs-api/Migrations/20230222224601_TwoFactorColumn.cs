using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuizaLabsApi.Migrations
{
    /// <inheritdoc />
    public partial class TwoFactorColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TwoFactorToken",
                table: "UserVerification",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoFactorToken",
                table: "UserVerification");
        }
    }
}
