using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternetMarket.Migrations
{
    public partial class HashToConnectionParams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "ConnectionParams",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "ConnectionParams",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIq+62Forpu7PKdqJb7HGFO7ILdJO+/HaMkDBpxxxT158K5GIjBQ+IpTapeul/jH4A==");

            migrationBuilder.InsertData(
                table: "ProductTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("439b466f-6156-4665-9388-c9773b3d3e77"), "Бытовая техника" },
                    { new Guid("be531799-ca13-4de9-8568-db5e8248e5c7"), "Смартфоны" },
                    { new Guid("392cd223-33df-41d6-8bbb-018cf81a5e65"), "Сад и огрод" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: new Guid("392cd223-33df-41d6-8bbb-018cf81a5e65"));

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: new Guid("439b466f-6156-4665-9388-c9773b3d3e77"));

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: new Guid("be531799-ca13-4de9-8568-db5e8248e5c7"));

            migrationBuilder.DropColumn(
                name: "Hash",
                table: "ConnectionParams");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "ConnectionParams");

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
    }
}
