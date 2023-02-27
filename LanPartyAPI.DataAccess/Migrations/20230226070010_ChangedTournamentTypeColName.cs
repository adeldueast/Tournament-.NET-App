using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LANPartyAPI_DataAccess.Migrations
{
    public partial class ChangedTournamentTypeColName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Tournaments",
                newName: "TournamentType");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "98109238-1a94-41da-beee-bd919c5efccc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32ffcfa8-7337-4d05-a17e-99d6280a12d5",
                column: "ConcurrencyStamp",
                value: "2f4b824b-4100-4761-bb8e-01ddebd158d8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3HEiEUpHH1eTYNzsYOOkjmK1o7Z2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f7914313-3a42-4790-9ba8-116705e27a6d", "AQAAAAEAACcQAAAAEN3K0ZNv5ndE6sqkUe7reztm3uaF2DVWVjBpMXt6AVb8jdv5m5qcFScejUYaY1Vsmw==", "6e9469cc-22f1-4f3d-96f8-cde57f580dc1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TournamentType",
                table: "Tournaments",
                newName: "Type");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "ebd31355-ece1-4b07-8016-19880567b2bd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32ffcfa8-7337-4d05-a17e-99d6280a12d5",
                column: "ConcurrencyStamp",
                value: "75eb7bb7-2603-4d38-9fb1-eb0b209fbeb3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3HEiEUpHH1eTYNzsYOOkjmK1o7Z2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "35080985-f4e2-4888-9956-a900aa0a3907", "AQAAAAEAACcQAAAAEHuCbXKlHEuEIuDWd1qds9bUBV0aD10S3B6n3a5GYXIPvOIJUlls6KQ6Sm7JfKuq2Q==", "d988ad80-1736-4b9f-bba2-dc56b140032c" });
        }
    }
}
