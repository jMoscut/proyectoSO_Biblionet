using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigitalRepository.Server.Migrations
{
    /// <inheritdoc />
    public partial class Agregandodatausuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Description",
                value: "Visualizador");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Description",
                value: "Cargador");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Email",
                value: "super@gmail.com");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DateToken", "Email", "Name", "Number", "Password", "RecoveryToken", "Reset", "RolId", "State", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 2L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, null, "emerson@gmail.com", "Visualizador", "12345678", "$2a$12$86Ty8oUVWKPbU8JqCII9VO.FgM1C10dweQ4xKhM4jj1LWL9jwNu7.", "", false, 2L, 1, null, null },
                    { 3L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, null, "jackie@gmail.com", "Cargador", "12345678", "$2a$12$86Ty8oUVWKPbU8JqCII9VO.FgM1C10dweQ4xKhM4jj1LWL9jwNu7.", "", false, 3L, 1, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Description",
                value: "Digitalizador");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Description",
                value: "Carga Documentos");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Email",
                value: "pruebas.test29111999@gmail.com");
        }
    }
}
