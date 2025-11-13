using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teste_Xbits.Infra.Migrations
{
    /// <inheritdoc />
    public partial class EnforceUniqueNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "product_category_id",
                schema: "xbits",
                table: "Product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_Name_Unique",
                schema: "xbits",
                table: "ProductCategory",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_Name_Unique",
                schema: "xbits",
                table: "Product",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_Name_Unique",
                schema: "xbits",
                table: "ProductCategory");

            migrationBuilder.DropIndex(
                name: "IX_Product_Name_Unique",
                schema: "xbits",
                table: "Product");

            migrationBuilder.AlterColumn<long>(
                name: "product_category_id",
                schema: "xbits",
                table: "Product",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
