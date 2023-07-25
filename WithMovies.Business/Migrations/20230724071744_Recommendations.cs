using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WithMovies.Business.Migrations
{
    /// <inheritdoc />
    public partial class Recommendations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ExplicitelyLikedGenres",
                table: "RecommendationProfiles",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RecommendationProfileId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RecommendationProfileInputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating = table.Column<double>(type: "REAL", nullable: true),
                    Watched = table.Column<bool>(type: "INTEGER", nullable: false),
                    ViewedDetailsPage = table.Column<bool>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendationProfileInputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendationProfileInputs_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecommendationProfileInputs_RecommendationProfiles_ParentId",
                        column: x => x.ParentId,
                        principalTable: "RecommendationProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_AspNetUsers_RecommendationProfileId",
                table: "AspNetUsers",
                column: "RecommendationProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationProfileInputs_MovieId",
                table: "RecommendationProfileInputs",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationProfileInputs_ParentId",
                table: "RecommendationProfileInputs",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightedMovies_ParentId",
                table: "WeightedMovies",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RecommendationProfiles_RecommendationProfileId",
                table: "AspNetUsers",
                column: "RecommendationProfileId",
                principalTable: "RecommendationProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RecommendationProfiles_RecommendationProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "RecommendationProfileInputs");

            migrationBuilder.DropTable(
                name: "WeightedMovies");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RecommendationProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ExplicitelyLikedGenres",
                table: "RecommendationProfiles");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RecommendationProfileId",
                table: "AspNetUsers");
        }
    }
}
