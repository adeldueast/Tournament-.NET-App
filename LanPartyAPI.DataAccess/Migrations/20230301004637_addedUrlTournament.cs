using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LANPartyAPI_DataAccess.Migrations
{
    public partial class addedUrlTournament : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "eac42452-fe3e-4df5-a127-1cbcee2b2324");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32ffcfa8-7337-4d05-a17e-99d6280a12d5",
                column: "ConcurrencyStamp",
                value: "80381939-6b22-45ee-af4e-3a4200fae397");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3HEiEUpHH1eTYNzsYOOkjmK1o7Z2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "46862939-5c0f-4568-9835-34595c831c11", "AQAAAAEAACcQAAAAEHAzKvcBJwA3kIxrZ++8br2rByZ/N0Qdx0GTSz2Ld50Tg+4WWLUHACxNkQRUtvNUng==", "e72c5d42-e1b4-4d13-8e6e-0f8d53a6bc02" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Tournaments");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "1e804bca-4556-450a-9ebc-8112d3b8c5b5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32ffcfa8-7337-4d05-a17e-99d6280a12d5",
                column: "ConcurrencyStamp",
                value: "0ff720c2-41e1-48aa-82cf-dfd9c39a64c4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3HEiEUpHH1eTYNzsYOOkjmK1o7Z2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6c0869fb-725a-45ab-b0e9-a2dae0129d79", "AQAAAAEAACcQAAAAEKePjYOabgbB9trlv/n2ege2G13jR/bmXEyMoUWdEf/127piX1tISVFnd/6W97QjJA==", "5a8f5671-eecf-40f3-98cb-46c231a41a09" });
        }
    }
}
