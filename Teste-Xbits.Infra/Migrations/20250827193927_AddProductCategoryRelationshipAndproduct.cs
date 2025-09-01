using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teste_Xbits.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddProductCategoryRelationshipAndproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                schema: "xbits",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    product_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    has_expiration_date = table.Column<bool>(type: "bit", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    product_category_id = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.id);
                    table.ForeignKey(
                        name: "FK_Product_ProductCategory_product_category_id",
                        column: x => x.product_category_id,
                        principalSchema: "xbits",
                        principalTable: "ProductCategory",
                        principalColumn: "product_category_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_product_category_id",
                schema: "xbits",
                table: "Product",
                column: "product_category_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product",
                schema: "xbits");
        }
    }
}
