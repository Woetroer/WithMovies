using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WithMovies.Business.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieCollections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PosterPath = table.Column<string>(type: "TEXT", nullable: true),
                    BackdropPath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieCollections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecommendationProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendationProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    PostedTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CastMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CastId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreditsId = table.Column<int>(type: "INTEGER", nullable: false),
                    Character = table.Column<string>(type: "TEXT", nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfilePath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CastMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Credits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrewMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreditsId = table.Column<int>(type: "INTEGER", nullable: false),
                    Department = table.Column<int>(type: "INTEGER", nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    Job = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ProfilePath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewMembers_Credits_CreditsId",
                        column: x => x.CreditsId,
                        principalTable: "Credits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImdbId = table.Column<string>(type: "TEXT", nullable: false),
                    BelongsToCollectionId = table.Column<int>(type: "INTEGER", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Tagline = table.Column<string>(type: "TEXT", nullable: false),
                    OriginalLanguage = table.Column<string>(type: "TEXT", nullable: true),
                    OriginalTitle = table.Column<string>(type: "TEXT", nullable: false),
                    Adult = table.Column<bool>(type: "INTEGER", nullable: false),
                    Overview = table.Column<string>(type: "TEXT", nullable: false),
                    Budget = table.Column<int>(type: "INTEGER", nullable: false),
                    Genres = table.Column<string>(type: "TEXT", nullable: false),
                    HomePage = table.Column<string>(type: "TEXT", nullable: true),
                    PosterPath = table.Column<string>(type: "TEXT", nullable: true),
                    ProductionCountries = table.Column<string>(type: "TEXT", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Revenue = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Runtime = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    SpokenLanguages = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    VoteAverage = table.Column<double>(type: "REAL", nullable: false),
                    VoteCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Popularity = table.Column<double>(type: "REAL", nullable: false),
                    CastMemberId = table.Column<int>(type: "INTEGER", nullable: true),
                    CrewMemberId = table.Column<int>(type: "INTEGER", nullable: true),
                    KeywordId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_CastMembers_CastMemberId",
                        column: x => x.CastMemberId,
                        principalTable: "CastMembers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Movies_CrewMembers_CrewMemberId",
                        column: x => x.CrewMemberId,
                        principalTable: "CrewMembers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Movies_Keywords_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "Keywords",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Movies_MovieCollections_BelongsToCollectionId",
                        column: x => x.BelongsToCollectionId,
                        principalTable: "MovieCollections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductionCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    MovieId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionCompanies_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CastMembers_CreditsId",
                table: "CastMembers",
                column: "CreditsId");

            migrationBuilder.CreateIndex(
                name: "IX_Credits_MovieId",
                table: "Credits",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewMembers_CreditsId",
                table: "CrewMembers",
                column: "CreditsId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_BelongsToCollectionId",
                table: "Movies",
                column: "BelongsToCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CastMemberId",
                table: "Movies",
                column: "CastMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CrewMemberId",
                table: "Movies",
                column: "CrewMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_KeywordId",
                table: "Movies",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionCompanies_MovieId",
                table: "ProductionCompanies",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_CastMembers_Credits_CreditsId",
                table: "CastMembers",
                column: "CreditsId",
                principalTable: "Credits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Credits_Movies_MovieId",
                table: "Credits",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CastMembers_Credits_CreditsId",
                table: "CastMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_CrewMembers_Credits_CreditsId",
                table: "CrewMembers");

            migrationBuilder.DropTable(
                name: "ProductionCompanies");

            migrationBuilder.DropTable(
                name: "RecommendationProfiles");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Credits");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "CastMembers");

            migrationBuilder.DropTable(
                name: "CrewMembers");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "MovieCollections");
        }
    }
}
