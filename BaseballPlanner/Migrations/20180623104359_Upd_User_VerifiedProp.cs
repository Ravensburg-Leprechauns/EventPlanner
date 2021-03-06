﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ClubGrid.Migrations
{
    public partial class Upd_User_VerifiedProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "verified",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "verified",
                table: "AspNetUsers");
        }
    }
}
