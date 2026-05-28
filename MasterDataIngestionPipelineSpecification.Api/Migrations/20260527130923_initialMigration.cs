using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterDataIngestionPipelineSpecification.Api.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "log_customer_ingestion",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RequestTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RawPayload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HttpStatus = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ValidationDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProcessError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_customer_ingestion", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "log_item_ingestion",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RequestTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RawPayload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HttpStatus = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ValidationDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProcessError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_item_ingestion", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "M_BRAND",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_BRAND", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "M_CATEGORY",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_CATEGORY", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "M_CHANNEL",
                columns: table => new
                {
                    ChannelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ChannelName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_CHANNEL", x => x.ChannelId);
                });

            migrationBuilder.CreateTable(
                name: "M_PAYMENT_TERM",
                columns: table => new
                {
                    PaymentTermId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TermCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TermName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreditDays = table.Column<int>(type: "int", nullable: false),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_PAYMENT_TERM", x => x.PaymentTermId);
                });

            migrationBuilder.CreateTable(
                name: "M_REGION",
                columns: table => new
                {
                    RegionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RegionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_REGION", x => x.RegionId);
                });

            migrationBuilder.CreateTable(
                name: "M_UOM",
                columns: table => new
                {
                    UomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UomCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_UOM", x => x.UomId);
                });

            migrationBuilder.CreateTable(
                name: "T_ITEM",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SalesOrgCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BaseUOM = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsBatchEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ITEM", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_T_ITEM_M_BRAND_BrandId",
                        column: x => x.BrandId,
                        principalTable: "M_BRAND",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_ITEM_M_CATEGORY_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "M_CATEGORY",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "M_CITY",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_CITY", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_M_CITY_M_REGION_RegionId",
                        column: x => x.RegionId,
                        principalTable: "M_REGION",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_ITEM_UOM_CONVERSION",
                columns: table => new
                {
                    ConversionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UomId = table.Column<int>(type: "int", nullable: false),
                    ConversionFactor = table.Column<decimal>(type: "decimal(18,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ITEM_UOM_CONVERSION", x => x.ConversionId);
                    table.ForeignKey(
                        name: "FK_T_ITEM_UOM_CONVERSION_M_UOM_UomId",
                        column: x => x.UomId,
                        principalTable: "M_UOM",
                        principalColumn: "UomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_ITEM_UOM_CONVERSION_T_ITEM_ItemId",
                        column: x => x.ItemId,
                        principalTable: "T_ITEM",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_CUSTOMER",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreditDays = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CUSTOMER", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_T_CUSTOMER_M_CITY_CityId",
                        column: x => x.CityId,
                        principalTable: "M_CITY",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_T_CUSTOMER_M_REGION_RegionId",
                        column: x => x.RegionId,
                        principalTable: "M_REGION",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_M_BRAND_BrandCode",
                table: "M_BRAND",
                column: "BrandCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_M_CATEGORY_CategoryCode",
                table: "M_CATEGORY",
                column: "CategoryCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_M_CHANNEL_ChannelCode",
                table: "M_CHANNEL",
                column: "ChannelCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_M_CITY_CityCode",
                table: "M_CITY",
                column: "CityCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_M_CITY_RegionId",
                table: "M_CITY",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_M_PAYMENT_TERM_TermCode",
                table: "M_PAYMENT_TERM",
                column: "TermCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_M_REGION_RegionCode",
                table: "M_REGION",
                column: "RegionCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_M_UOM_UomCode",
                table: "M_UOM",
                column: "UomCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_CUSTOMER_CityId",
                table: "T_CUSTOMER",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_T_CUSTOMER_CustomerCode",
                table: "T_CUSTOMER",
                column: "CustomerCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_CUSTOMER_RegionId",
                table: "T_CUSTOMER",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_T_ITEM_BrandId",
                table: "T_ITEM",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_T_ITEM_CategoryId",
                table: "T_ITEM",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_T_ITEM_ItemCode",
                table: "T_ITEM",
                column: "ItemCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_ITEM_UOM_CONVERSION_ItemId_UomId",
                table: "T_ITEM_UOM_CONVERSION",
                columns: new[] { "ItemId", "UomId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_ITEM_UOM_CONVERSION_UomId",
                table: "T_ITEM_UOM_CONVERSION",
                column: "UomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "log_customer_ingestion");

            migrationBuilder.DropTable(
                name: "log_item_ingestion");

            migrationBuilder.DropTable(
                name: "M_CHANNEL");

            migrationBuilder.DropTable(
                name: "M_PAYMENT_TERM");

            migrationBuilder.DropTable(
                name: "T_CUSTOMER");

            migrationBuilder.DropTable(
                name: "T_ITEM_UOM_CONVERSION");

            migrationBuilder.DropTable(
                name: "M_CITY");

            migrationBuilder.DropTable(
                name: "M_UOM");

            migrationBuilder.DropTable(
                name: "T_ITEM");

            migrationBuilder.DropTable(
                name: "M_REGION");

            migrationBuilder.DropTable(
                name: "M_BRAND");

            migrationBuilder.DropTable(
                name: "M_CATEGORY");
        }
    }
}
