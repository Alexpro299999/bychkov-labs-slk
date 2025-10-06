using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WatchStore.DataAccess.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ManufacturerName = table.Column<string>(type: "TEXT", nullable: true),
                    Country = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Watches",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WatchModel = table.Column<string>(type: "TEXT", nullable: true),
                    WatchType = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    ManufacturerID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watches", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Watches_Manufacturers_ManufacturerID",
                        column: x => x.ManufacturerID,
                        principalTable: "Manufacturers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeatureWatch",
                columns: table => new
                {
                    FeaturesID = table.Column<int>(type: "INTEGER", nullable: false),
                    WatchesID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureWatch", x => new { x.FeaturesID, x.WatchesID });
                    table.ForeignKey(
                        name: "FK_FeatureWatch_Features_FeaturesID",
                        column: x => x.FeaturesID,
                        principalTable: "Features",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureWatch_Watches_WatchesID",
                        column: x => x.WatchesID,
                        principalTable: "Watches",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WatchID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Stock_Watches_WatchID",
                        column: x => x.WatchID,
                        principalTable: "Watches",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "ID", "Country", "ManufacturerName" },
                values: new object[,]
                {
                    { 1, "Япония", "Casio" },
                    { 2, "Швейцария", "Tissot" },
                    { 3, "Швейцария", "Rolex" }
                });

            migrationBuilder.InsertData(
                table: "Watches",
                columns: new[] { "ID", "ManufacturerID", "Price", "WatchModel", "WatchType" },
                values: new object[,]
                {
                    { 1, 1, 12000m, "G-Shock GA-2100", "кварцевые" },
                    { 2, 2, 45000m, "T-Classic", "механические" },
                    { 3, 3, 850000m, "Submariner", "механические" }
                });

            migrationBuilder.InsertData(
                table: "Stock",
                columns: new[] { "ID", "DeliveryDate", "Quantity", "WatchID" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, 1 },
                    { 2, new DateTime(2023, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 2 },
                    { 3, new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureWatch_WatchesID",
                table: "FeatureWatch",
                column: "WatchesID");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_WatchID",
                table: "Stock",
                column: "WatchID");

            migrationBuilder.CreateIndex(
                name: "IX_Watches_ManufacturerID",
                table: "Watches",
                column: "ManufacturerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureWatch");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "Watches");

            migrationBuilder.DropTable(
                name: "Manufacturers");
        }
    }
}
