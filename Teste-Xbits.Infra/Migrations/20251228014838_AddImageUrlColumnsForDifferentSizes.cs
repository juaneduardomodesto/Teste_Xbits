using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teste_Xbits.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlColumnsForDifferentSizes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                schema: "xbits",
                table: "ImageFiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LargeUrl",
                schema: "xbits",
                table: "ImageFiles",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediumUrl",
                schema: "xbits",
                table: "ImageFiles",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalUrl",
                schema: "xbits",
                table: "ImageFiles",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                schema: "xbits",
                table: "ImageFiles",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LargeUrl",
                schema: "xbits",
                table: "ImageFiles");

            migrationBuilder.DropColumn(
                name: "MediumUrl",
                schema: "xbits",
                table: "ImageFiles");

            migrationBuilder.DropColumn(
                name: "OriginalUrl",
                schema: "xbits",
                table: "ImageFiles");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                schema: "xbits",
                table: "ImageFiles");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                schema: "xbits",
                table: "ImageFiles",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
