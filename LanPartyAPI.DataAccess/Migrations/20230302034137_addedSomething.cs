using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LANPartyAPI_DataAccess.Migrations
{
    public partial class addedSomething : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "ab867df7-6765-4bc1-ac06-8aade8c097ad");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32ffcfa8-7337-4d05-a17e-99d6280a12d5",
                column: "ConcurrencyStamp",
                value: "c6533fb4-2f61-4009-8986-5d28af342596");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3HEiEUpHH1eTYNzsYOOkjmK1o7Z2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a848016-4c95-48ad-8428-cd7358140c5c", "AQAAAAEAACcQAAAAEN+5WZk7S4UMLY+BiFEH0N3MTmbr5CRs1nFXKzsWLRTALXZiF0CUx0qS0M0JtcpMSw==", "6d042ea9-a862-4896-9848-87bd6995f3d5" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
