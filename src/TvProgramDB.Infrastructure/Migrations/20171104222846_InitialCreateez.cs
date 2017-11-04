using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TvProgramDB.Infrastructure.Migrations
{
    public partial class InitialCreateez : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChanelId",
                table: "Program",
                type: "int4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Program_ChanelId",
                table: "Program",
                column: "ChanelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Program_Chanel_ChanelId",
                table: "Program",
                column: "ChanelId",
                principalTable: "Chanel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Program_Chanel_ChanelId",
                table: "Program");

            migrationBuilder.DropIndex(
                name: "IX_Program_ChanelId",
                table: "Program");

            migrationBuilder.DropColumn(
                name: "ChanelId",
                table: "Program");
        }
    }
}
