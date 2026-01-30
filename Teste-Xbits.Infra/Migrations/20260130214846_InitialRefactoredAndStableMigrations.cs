using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teste_Xbits.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitialRefactoredAndStableMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "xbits");

            migrationBuilder.CreateTable(
                name: "ImageFiles",
                schema: "xbits",
                columns: table => new
                {
                    image_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    file_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    storage_path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    content_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    size_in_bytes = table.Column<long>(type: "bigint", nullable: false),
                    image_type = table.Column<int>(type: "int", nullable: false),
                    entity_type = table.Column<int>(type: "int", nullable: false),
                    entity_id = table.Column<long>(type: "bigint", nullable: false),
                    display_order = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    is_main = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    alt_text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    original_url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    thumbnail_url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    medium_url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    large_url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageFiles", x => x.image_id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                schema: "xbits",
                columns: table => new
                {
                    product_category_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    product_category_code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.product_category_id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "xbits",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    accept_privacy_policy = table.Column<bool>(type: "bit", nullable: false),
                    accept_terms_of_use = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    role = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.user_id);
                });

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
                    product_category_id = table.Column<long>(type: "bigint", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                schema: "xbits",
                columns: table => new
                {
                    cart_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    checked_out_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.cart_id);
                    table.ForeignKey(
                        name: "FK_Cart_User_user_id",
                        column: x => x.user_id,
                        principalSchema: "xbits",
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DomainLogger",
                schema: "xbits",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    entity_id = table.Column<string>(type: "nvarchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainLogger", x => x.id);
                    table.ForeignKey(
                        name: "FK_DomainLogger_User_user_id",
                        column: x => x.user_id,
                        principalSchema: "xbits",
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                schema: "xbits",
                columns: table => new
                {
                    cart_item_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cart_id = table.Column<long>(type: "bigint", nullable: false),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.cart_item_id);
                    table.ForeignKey(
                        name: "FK_CartItem_Cart_cart_id",
                        column: x => x.cart_id,
                        principalSchema: "xbits",
                        principalTable: "Cart",
                        principalColumn: "cart_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItem_Product_product_id",
                        column: x => x.product_id,
                        principalSchema: "xbits",
                        principalTable: "Product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "xbits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CartId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserId1 = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Cart_CartId",
                        column: x => x.CartId,
                        principalSchema: "xbits",
                        principalTable: "Cart",
                        principalColumn: "cart_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "xbits",
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_User_UserId1",
                        column: x => x.UserId1,
                        principalSchema: "xbits",
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                schema: "xbits",
                columns: table => new
                {
                    order_item_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<long>(type: "bigint", nullable: false),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    product_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.order_item_id);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_order_id",
                        column: x => x.order_id,
                        principalSchema: "xbits",
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_Product_product_id",
                        column: x => x.product_id,
                        principalSchema: "xbits",
                        principalTable: "Product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserId_Status",
                schema: "xbits",
                table: "Cart",
                columns: new[] { "user_id", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId_ProductId",
                schema: "xbits",
                table: "CartItem",
                columns: new[] { "cart_id", "product_id" });

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_product_id",
                schema: "xbits",
                table: "CartItem",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_DomainLogger_user_id",
                schema: "xbits",
                table: "DomainLogger",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_ImageFiles_EntityType_EntityId",
                schema: "xbits",
                table: "ImageFiles",
                columns: new[] { "entity_type", "entity_id" });

            migrationBuilder.CreateIndex(
                name: "IX_ImageFiles_EntityType_EntityId_IsMain",
                schema: "xbits",
                table: "ImageFiles",
                columns: new[] { "entity_type", "entity_id", "is_main" });

            migrationBuilder.CreateIndex(
                name: "IX_ImageFiles_ImageType",
                schema: "xbits",
                table: "ImageFiles",
                column: "image_type");

            migrationBuilder.CreateIndex(
                name: "IX_ImageFiles_StoragePath",
                schema: "xbits",
                table: "ImageFiles",
                column: "storage_path");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CartId",
                schema: "xbits",
                table: "Order",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderNumber",
                schema: "xbits",
                table: "Order",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_Status",
                schema: "xbits",
                table: "Order",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                schema: "xbits",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId1",
                schema: "xbits",
                table: "Order",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                schema: "xbits",
                table: "OrderItem",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId_ProductId",
                schema: "xbits",
                table: "OrderItem",
                columns: new[] { "order_id", "product_id" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_product_id",
                schema: "xbits",
                table: "OrderItem",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Name_Unique",
                schema: "xbits",
                table: "Product",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_product_category_id",
                schema: "xbits",
                table: "Product",
                column: "product_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_Name_Unique",
                schema: "xbits",
                table: "ProductCategory",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_ProductCategoryCode_Unique",
                schema: "xbits",
                table: "ProductCategory",
                column: "product_category_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItem",
                schema: "xbits");

            migrationBuilder.DropTable(
                name: "DomainLogger",
                schema: "xbits");

            migrationBuilder.DropTable(
                name: "ImageFiles",
                schema: "xbits");

            migrationBuilder.DropTable(
                name: "OrderItem",
                schema: "xbits");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "xbits");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "xbits");

            migrationBuilder.DropTable(
                name: "Cart",
                schema: "xbits");

            migrationBuilder.DropTable(
                name: "ProductCategory",
                schema: "xbits");

            migrationBuilder.DropTable(
                name: "User",
                schema: "xbits");
        }
    }
}
