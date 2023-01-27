using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InternetMarket.Migrations
{
    public partial class AddIsByingToShoppungCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: new Guid("1dddd475-e10e-40ff-8d03-633f3e4977e6"));

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: new Guid("7ada282e-a582-47f9-9878-c06c835b427f"));

            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "Id",
                keyValue: new Guid("b26bae64-75fd-4d90-8388-d246f989ae84"));

            migrationBuilder.AddColumn<bool>(
                name: "IsBying",
                table: "ShoppingCart",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fab4fac1-c546-41de-aebc-a14da6895711",
                column: "NormalizedName",
                value: "ADMIN");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "avadvd", true, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEPfUK/xHZEcTHnj66MVSsbgp4qOPLSnRrt+GpSE7ie9p+tfGlFCJOY3ndHWxfxHs6Q==", "avebgdfvs" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "IsBying",
                table: "ShoppingCart");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fab4fac1-c546-41de-aebc-a14da6895711",
                column: "NormalizedName",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5511ecee-31a2-4391-bc29-90714df4de92", false, null, null, "AQAAAAEAACcQAAAAEJ84An6qdJHdm1SEWYSTwpjha+iEgOVp/jZWT/sS36uewUDA56Wg3hL6vr9azgFMKw==", "777cc4df-8ee1-4a47-942c-2167d0f4ea0d" });

            migrationBuilder.InsertData(
                table: "ProductTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("7ada282e-a582-47f9-9878-c06c835b427f"), "Бытовая техника" },
                    { new Guid("1dddd475-e10e-40ff-8d03-633f3e4977e6"), "Игрушки" },
                    { new Guid("b26bae64-75fd-4d90-8388-d246f989ae84"), "Машины" }
                });
        }
    }
}
