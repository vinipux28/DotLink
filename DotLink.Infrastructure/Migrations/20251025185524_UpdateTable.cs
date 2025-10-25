using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Posts_PostId",
                table: "Votes");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Users_UserId",
                table: "Votes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Votes",
                table: "Votes");

            migrationBuilder.RenameTable(
                name: "Votes",
                newName: "PostVotes");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_UserId",
                table: "PostVotes",
                newName: "IX_PostVotes_UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PostVotes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PostVotes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostVotes",
                table: "PostVotes",
                columns: new[] { "PostId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PostVotes_Posts_PostId",
                table: "PostVotes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostVotes_Users_UserId",
                table: "PostVotes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostVotes_Posts_PostId",
                table: "PostVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PostVotes_Users_UserId",
                table: "PostVotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostVotes",
                table: "PostVotes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PostVotes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PostVotes");

            migrationBuilder.RenameTable(
                name: "PostVotes",
                newName: "Votes");

            migrationBuilder.RenameIndex(
                name: "IX_PostVotes_UserId",
                table: "Votes",
                newName: "IX_Votes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Votes",
                table: "Votes",
                columns: new[] { "PostId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Posts_PostId",
                table: "Votes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Users_UserId",
                table: "Votes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
