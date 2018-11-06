using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Insight.SaveTheBees.SelfServe.WebApi.Data.Migrations.ApplicationDatabase
{
    public partial class AddHiveAndClusterTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clusters",
                columns: table => new
                {
                    ClusterId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clusters", x => x.ClusterId);
                    table.ForeignKey(
                        name: "FK_Clusters_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hives",
                columns: table => new
                {
                    HiveId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ClusterId = table.Column<Guid>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hives", x => x.HiveId);
                    table.ForeignKey(
                        name: "FK_Hives_Clusters_ClusterId",
                        column: x => x.ClusterId,
                        principalTable: "Clusters",
                        principalColumn: "ClusterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hives_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clusters_UserId",
                table: "Clusters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Hives_ClusterId",
                table: "Hives",
                column: "ClusterId");

            migrationBuilder.CreateIndex(
                name: "IX_Hives_UserId",
                table: "Hives",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hives");

            migrationBuilder.DropTable(
                name: "Clusters");
        }
    }
}
