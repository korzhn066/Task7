using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task7.Migrations
{
    public partial class ChangeMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8c66fd82-6a9a-4d59-80c2-7fd2871023c8", "AQAAAAEAACcQAAAAEIQxb+p08mR0Tw4zIK8038Hkm+TnVKOFr8fkwlbeD6Xypf/gW/elbjYJWHYVM+fvMA==", "88d50940-631d-4684-ab51-af0c1f4a947e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "82746017-1bb1-451c-888c-f90f8ea8c4fa", "AQAAAAEAACcQAAAAECEusVhqIyOA2c6R31zaBA+d9BifRUqs+5gVn8RjNU2z/sJOfdO3k2QpunCdvGasvA==", "4d04d586-a7e8-4bdb-97fd-3e7b0190af6c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "819e8a93-74b8-4a74-8706-862b0c2a2b11", "AQAAAAEAACcQAAAAELYff0P+gRzIFXJZcGIzI/idNhhh5kzX/BDL+C/BUsHjdn5bdmaBPdYrwO+pPwfCVQ==", "6f549536-9c4f-4ada-b0f0-e938e121f81d" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Sender",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "40c5b96a-5e1a-4d7b-987b-237d31339310", "AQAAAAEAACcQAAAAEG7VCR7PPrbJo7aNC7v3azsURimkPw8LMyf+FR0quvHlRGoW2a5/I/ORYzRREPz8Ug==", "4156f41e-77a0-493c-82e8-a65a1a63b0c2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c569b52b-f616-49ee-9729-e6d67ab5244a", "AQAAAAEAACcQAAAAEEp8fEidH8IvskamZqSilPDzIPj+72gH71jCPUikw2vAc3Fp6uilRIQT6tYoILDs3g==", "e11a34bc-e261-415c-8b27-22aca3f69b10" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0a2748a3-8fee-477d-836d-4861c3b26465", "AQAAAAEAACcQAAAAEAbyxRirtuc3+z+jkz443MMT75JzwuNf8RWjoWv+JLAPKbfgRZVvWNV/FqExuIjIcQ==", "b0c3a3e8-4d84-4032-b10c-2bc736596120" });
        }
    }
}
