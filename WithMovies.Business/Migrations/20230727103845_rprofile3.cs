using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WithMovies.Business.Migrations
{
    /// <inheritdoc />
    public partial class rprofile3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeightedMovies");

            migrationBuilder.AddColumn<bool>(
                name: "CanReview",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "WeightedKeywords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    KeywordId = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightedKeywords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightedKeywords_Keywords_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "Keywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeightedKeywords_RecommendationProfiles_ParentId",
                        column: x => x.ParentId,
                        principalTable: "RecommendationProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeightedKeywords_KeywordId",
                table: "WeightedKeywords",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightedKeywords_ParentId",
                table: "WeightedKeywords",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeightedKeywords");

            migrationBuilder.DropColumn(
                name: "CanReview",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "WeightedMovies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightedMovies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightedMovies_RecommendationProfiles_ParentId",
                        column: x => x.ParentId,
                        principalTable: "RecommendationProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeightedMovies_ParentId",
                table: "WeightedMovies",
                column: "ParentId");
        }
    }
}
