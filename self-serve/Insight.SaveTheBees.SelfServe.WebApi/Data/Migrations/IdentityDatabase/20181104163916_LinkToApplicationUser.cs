using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Insight.SaveTheBees.SelfServe.WebApi.Data.Migrations.IdentityDatabase
{
    public partial class LinkToApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "AspNetUsers");
        }
    }
}
