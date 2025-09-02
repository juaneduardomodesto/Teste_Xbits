using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teste_Xbits.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleColummnToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "role",
                schema: "xbits",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                schema: "xbits",
                table: "User");
        }
    }
}
