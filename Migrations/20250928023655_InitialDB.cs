using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogServiceAPI_Electric_Store.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "attributes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    data_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attributes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "brands",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    slug = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false, defaultValue: "default-slug"),
                    status = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__brands__3213E83FBBDEC841", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false),
                    parent_id = table.Column<int>(type: "int", nullable: true),
                    path = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    level = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(sysutcdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__3214EC0735611CED", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nchar(150)", fixedLength: true, maxLength: 150, nullable: false),
                    phone = table.Column<string>(type: "nchar(11)", fixedLength: true, maxLength: 11, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    role = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__3213E83FA25F67D3", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "category_attributes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_id = table.Column<int>(type: "int", nullable: false),
                    attribute_id = table.Column<int>(type: "int", nullable: false),
                    is_filterable = table.Column<bool>(type: "bit", nullable: false),
                    is_variant_level = table.Column<bool>(type: "bit", nullable: false),
                    is_required = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_attributes", x => x.id);
                    table.ForeignKey(
                        name: "FK_category_attributes_attributes",
                        column: x => x.attribute_id,
                        principalTable: "attributes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_category_attributes_categories",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "nchar(100)", fixedLength: true, maxLength: 100, nullable: false),
                    brand_id = table.Column<int>(type: "int", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nchar(1000)", fixedLength: true, maxLength: 1000, nullable: false),
                    rating = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_products_brands",
                        column: x => x.brand_id,
                        principalTable: "brands",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_products_categories",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product_attribute",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    attribute_id = table.Column<int>(type: "int", nullable: false),
                    value_text = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    value_decimal = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    value_int = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_attribute", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_attribute_attributes",
                        column: x => x.attribute_id,
                        principalTable: "attributes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_product_attribute_products",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_variants",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: false, defaultValue: "default"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    SKU = table.Column<string>(type: "nchar(100)", fixedLength: true, maxLength: 100, nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_variants", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_variants_products",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product_image",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    variant_id = table.Column<int>(type: "int", nullable: false),
                    url = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_image", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_image_product_variants",
                        column: x => x.variant_id,
                        principalTable: "product_variants",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "variant_attribute",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    attribute_id = table.Column<int>(type: "int", nullable: false),
                    variant_id = table.Column<int>(type: "int", nullable: false),
                    value_int = table.Column<int>(type: "int", nullable: true),
                    value_decimal = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    value_text = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variant_attribute", x => x.id);
                    table.ForeignKey(
                        name: "FK_variant_attribute_attributes",
                        column: x => x.attribute_id,
                        principalTable: "attributes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_variant_attribute_product_variants",
                        column: x => x.variant_id,
                        principalTable: "product_variants",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Categori__BC7B5FB6F07DC5B5",
                table: "categories",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_category_attributes_attribute_id",
                table: "category_attributes",
                column: "attribute_id");

            migrationBuilder.CreateIndex(
                name: "IX_category_attributes_category_id",
                table: "category_attributes",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_attribute_attribute_id",
                table: "product_attribute",
                column: "attribute_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_attribute_product_id",
                table: "product_attribute",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_image_variant_id",
                table: "product_image",
                column: "variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_variants_product_id",
                table: "product_variants",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_brand_id",
                table: "products",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_category_id",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_variant_attribute_attribute_id",
                table: "variant_attribute",
                column: "attribute_id");

            migrationBuilder.CreateIndex(
                name: "IX_variant_attribute_variant_id",
                table: "variant_attribute",
                column: "variant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "category_attributes");

            migrationBuilder.DropTable(
                name: "product_attribute");

            migrationBuilder.DropTable(
                name: "product_image");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "variant_attribute");

            migrationBuilder.DropTable(
                name: "attributes");

            migrationBuilder.DropTable(
                name: "product_variants");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "brands");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
