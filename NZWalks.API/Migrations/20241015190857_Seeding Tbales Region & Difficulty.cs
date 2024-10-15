using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingTbalesRegionDifficulty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("539f569a-1044-44e0-94b1-4c68834f6d1d"), "Hard" },
                    { new Guid("66f0a310-3f96-4298-a126-adf7fa491c79"), "Medium" },
                    { new Guid("9e336c61-faed-4281-82d9-639537cd1d4a"), "Easy" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("39deb490-e199-48ca-80d5-d6ccdd21ab18"), "AWK", "Auckland", "regionurl1.jpg" },
                    { new Guid("4e86ea9f-e912-4a82-bf40-af1085bff5a4"), "SAC", "Sergio", "regionurl3.jpg" },
                    { new Guid("ff7397f5-2cb3-41d1-b2e6-1ad96bc0c927"), "NES", "Nessieuwu", "regionurl2.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("539f569a-1044-44e0-94b1-4c68834f6d1d"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("66f0a310-3f96-4298-a126-adf7fa491c79"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("9e336c61-faed-4281-82d9-639537cd1d4a"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("39deb490-e199-48ca-80d5-d6ccdd21ab18"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("4e86ea9f-e912-4a82-bf40-af1085bff5a4"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("ff7397f5-2cb3-41d1-b2e6-1ad96bc0c927"));
        }
    }
}
