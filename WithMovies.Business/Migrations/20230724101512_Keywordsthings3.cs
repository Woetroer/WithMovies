using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WithMovies.Business.Migrations
{
    /// <inheritdoc />
    public partial class Keywordsthings3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Keywords_KeywordId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_KeywordId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "KeywordId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "Movies");

            migrationBuilder.CreateTable(
                name: "KeywordMovie",
                columns: table => new
                {
                    KeywordsId = table.Column<int>(type: "INTEGER", nullable: false),
                    MoviesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordMovie", x => new { x.KeywordsId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_KeywordMovie_Keywords_KeywordsId",
                        column: x => x.KeywordsId,
                        principalTable: "Keywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KeywordMovie_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeywordMovie_MoviesId",
                table: "KeywordMovie",
                column: "MoviesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeywordMovie");

            migrationBuilder.AddColumn<int>(
                name: "KeywordId",
                table: "Movies",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "Movies",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_KeywordId",
                table: "Movies",
                column: "KeywordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Keywords_KeywordId",
                table: "Movies",
                column: "KeywordId",
                principalTable: "Keywords",
                principalColumn: "Id");
        }
    }
}
