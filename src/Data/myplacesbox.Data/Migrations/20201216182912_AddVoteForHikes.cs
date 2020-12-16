using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPlacesBox.Data.Migrations
{
    public partial class AddVoteForHikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HikeVote_AspNetUsers_UserId",
                table: "HikeVote");

            migrationBuilder.DropForeignKey(
                name: "FK_HikeVote_Hikes_HikeId",
                table: "HikeVote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HikeVote",
                table: "HikeVote");

            migrationBuilder.RenameTable(
                name: "HikeVote",
                newName: "HikeVotes");

            migrationBuilder.RenameIndex(
                name: "IX_HikeVote_UserId",
                table: "HikeVotes",
                newName: "IX_HikeVotes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_HikeVote_HikeId",
                table: "HikeVotes",
                newName: "IX_HikeVotes_HikeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HikeVotes",
                table: "HikeVotes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HikeVotes_AspNetUsers_UserId",
                table: "HikeVotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HikeVotes_Hikes_HikeId",
                table: "HikeVotes",
                column: "HikeId",
                principalTable: "Hikes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HikeVotes_AspNetUsers_UserId",
                table: "HikeVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_HikeVotes_Hikes_HikeId",
                table: "HikeVotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HikeVotes",
                table: "HikeVotes");

            migrationBuilder.RenameTable(
                name: "HikeVotes",
                newName: "HikeVote");

            migrationBuilder.RenameIndex(
                name: "IX_HikeVotes_UserId",
                table: "HikeVote",
                newName: "IX_HikeVote_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_HikeVotes_HikeId",
                table: "HikeVote",
                newName: "IX_HikeVote_HikeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HikeVote",
                table: "HikeVote",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HikeVote_AspNetUsers_UserId",
                table: "HikeVote",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HikeVote_Hikes_HikeId",
                table: "HikeVote",
                column: "HikeId",
                principalTable: "Hikes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
