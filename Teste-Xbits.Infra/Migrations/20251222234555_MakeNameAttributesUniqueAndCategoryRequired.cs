using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teste_Xbits.Infra.Migrations
{
    /// <inheritdoc />
    public partial class MakeNameAttributesUniqueAndCategoryRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategory_product_category_id",
                schema: "xbits",
                table: "Product");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "xbits",
                table: "ProductCategory",
                type: "nvarchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
                name: "IX_ProductCategory_Name_Unique",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategory_product_category_id",
                schema: "xbits",
                table: "Product",
                column: "product_category_id",
                principalSchema: "xbits",
                principalTable: "ProductCategory",
                principalColumn: "product_category_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategory_product_category_id",
                schema: "xbits",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategory_Name_Unique",
                schema: "xbits",
                table: "ProductCategory");

            migrationBuilder.DropIndex(
                name: "IX_Product_Name_Unique",
                schema: "xbits",
                table: "Product");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "xbits",
                table: "ProductCategory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)");

            migrationBuilder.AlterColumn<long>(
                name: "product_category_id",
                schema: "xbits",
                table: "Product",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategory_product_category_id",
                schema: "xbits",
                table: "Product",
                column: "product_category_id",
                principalSchema: "xbits",
                principalTable: "ProductCategory",
                principalColumn: "product_category_id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
