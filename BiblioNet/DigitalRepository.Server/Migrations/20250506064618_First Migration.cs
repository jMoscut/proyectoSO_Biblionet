using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigitalRepository.Server.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Policy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Icon = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    IsVisible = table.Column<bool>(type: "boolean", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RolId = table.Column<long>(type: "bigint", nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    RecoveryToken = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DateToken = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Reset = table.Column<bool>(type: "boolean", nullable: true),
                    Number = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolOperations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RolId = table.Column<long>(type: "bigint", nullable: false),
                    OperationId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolOperations_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolOperations_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "Image", "Name", "Path", "State", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, "Modulo de Archivos", "file", "Archivos", "Files", 1, null, null });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "Name", "State", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, "Super Administrador", "SA", 1, null, null },
                    { 2L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, "Digitalizador", "DG", 1, null, null },
                    { 3L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, "Carga Documentos", "CD", 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "Guid", "Icon", "IsVisible", "ModuleId", "Name", "Path", "Policy", "State", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, "Consultar Documentos", "6451551a-b05c-455b-b1b9-97616e1c8892", "list", true, 1L, "Listar Documentos", "Files", "Files.List", 1, null, null },
                    { 2L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, "Creacion de documentos", "2e26b4ca-bd5d-4c4f-a027-ba09f5bd448f", "plus", false, 1L, "Cargar Documento", "Files/Create", "Files.Create", 1, null, null },
                    { 3L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, "Descarga de documentos", "3fd82baf-a73d-4809-8508-60dbec6119b0", "download", true, 1L, "Descargar Documento", "Files/Download", "Files.Download", 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DateToken", "Email", "Name", "Number", "Password", "RecoveryToken", "Reset", "RolId", "State", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, null, "pruebas.test29111999@gmail.com", "Super Administrador", "12345678", "$2a$12$86Ty8oUVWKPbU8JqCII9VO.FgM1C10dweQ4xKhM4jj1LWL9jwNu7.", "", false, 1L, 1, null, null });

            migrationBuilder.InsertData(
                table: "RolOperations",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "OperationId", "RolId", "State", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, 1L, 1L, 1, null, null },
                    { 2L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, 2L, 1L, 1, null, null },
                    { 3L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, 3L, 1L, 1, null, null },
                    { 4L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, 1L, 2L, 1, null, null },
                    { 5L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, 2L, 2L, 1, null, null },
                    { 6L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, 1L, 3L, 1, null, null },
                    { 7L, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1L, 3L, 3L, 1, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operations_ModuleId",
                table: "Operations",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolOperations_OperationId",
                table: "RolOperations",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_RolOperations_RolId",
                table: "RolOperations",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RolId",
                table: "Users",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolOperations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Modules");
        }
    }
}
