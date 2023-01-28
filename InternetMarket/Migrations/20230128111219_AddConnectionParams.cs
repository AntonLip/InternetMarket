using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternetMarket.Migrations
{
    public partial class AddConnectionParams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: new Guid("0b28d4b9-bb8f-4097-8976-02f6e920c3a2"));

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: new Guid("2e24b8dc-69a0-4b17-9991-a66d022e9831"));

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: new Guid("47ee1ad9-dcf8-406b-9a01-8e3ca3cb66d2"));

            migrationBuilder.AddColumn<string>(
                name: "ArticleNumber",
                table: "Products",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConnectionParams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RemoteIpAddr = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserAgent = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConnectionTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TimeToCaptch = table.Column<int>(type: "int", nullable: false),
                    RemotePort = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsBot = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionParams", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECAU6sQcb7tUXFJNTrsbJYpqSjHup+pZWLrsAOSU9Bo2rTWelM8lFrGGgTvgNYRqbw==");

            migrationBuilder.InsertData(
                table: "ProductTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("60d89ad4-9c9c-4e5f-80f3-d5a4a71cd34e"), "Бытовая техника" },
                    { new Guid("dfaf75dd-a675-44ae-9335-ecabdad8e8a2"), "Смартфоны" },
                    { new Guid("3254b2d3-2ff9-44cd-998c-70db30442efa"), "Сад и огрод" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionParams");

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: new Guid("3254b2d3-2ff9-44cd-998c-70db30442efa"));

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: new Guid("60d89ad4-9c9c-4e5f-80f3-d5a4a71cd34e"));

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: new Guid("dfaf75dd-a675-44ae-9335-ecabdad8e8a2"));

            migrationBuilder.DropColumn(
                name: "ArticleNumber",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEPfUK/xHZEcTHnj66MVSsbgp4qOPLSnRrt+GpSE7ie9p+tfGlFCJOY3ndHWxfxHs6Q==");

            migrationBuilder.InsertData(
                table: "ProductTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0b28d4b9-bb8f-4097-8976-02f6e920c3a2"), "Бытовая техника" },
                    { new Guid("2e24b8dc-69a0-4b17-9991-a66d022e9831"), "Смартфоны" },
                    { new Guid("47ee1ad9-dcf8-406b-9a01-8e3ca3cb66d2"), "Сад и огрод" }
                });
        }
    }
}
