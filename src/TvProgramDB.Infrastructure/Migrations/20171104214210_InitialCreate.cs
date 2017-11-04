using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TvProgramDB.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "Chanel_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "ChanelName_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "Country_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "Program_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "Source_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false),
                    Code = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Program",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false),
                    Description = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ProgramType = table.Column<int>(type: "int4", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Program", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Source",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false),
                    Code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Source", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chanel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false),
                    CountryId = table.Column<int>(type: "int4", nullable: true),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chanel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chanel_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChanelName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false),
                    ChanelId = table.Column<int>(type: "int4", nullable: true),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChanelName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChanelName_Chanel_ChanelId",
                        column: x => x.ChanelId,
                        principalTable: "Chanel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chanel_CountryId",
                table: "Chanel",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Chanel_Name",
                table: "Chanel",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChanelName_ChanelId",
                table: "ChanelName",
                column: "ChanelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChanelName_Name",
                table: "ChanelName",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Country_Code",
                table: "Country",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Country_Name",
                table: "Country",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Source_Code",
                table: "Source",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Source_Name",
                table: "Source",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChanelName");

            migrationBuilder.DropTable(
                name: "Program");

            migrationBuilder.DropTable(
                name: "Source");

            migrationBuilder.DropTable(
                name: "Chanel");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropSequence(
                name: "Chanel_hilo");

            migrationBuilder.DropSequence(
                name: "ChanelName_hilo");

            migrationBuilder.DropSequence(
                name: "Country_hilo");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence");

            migrationBuilder.DropSequence(
                name: "Program_hilo");

            migrationBuilder.DropSequence(
                name: "Source_hilo");
        }
    }
}
