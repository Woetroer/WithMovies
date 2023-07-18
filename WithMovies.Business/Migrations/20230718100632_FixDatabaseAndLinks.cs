using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WithMovies.Business.Migrations
{
    /// <inheritdoc />
    public partial class FixDatabaseAndLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CastMembers_Credits_CreditsId",
                table: "CastMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_CrewMembers_Credits_CreditsId",
                table: "CrewMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_CastMembers_CastMemberId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_CrewMembers_CrewMemberId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionCompanies_Movies_MovieId",
                table: "ProductionCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId1",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "Credits");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_ProductionCompanies_MovieId",
                table: "ProductionCompanies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_CastMemberId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_CrewMemberId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_CrewMembers_CreditsId",
                table: "CrewMembers");

            migrationBuilder.DropIndex(
                name: "IX_CastMembers_CreditsId",
                table: "CastMembers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "ProductionCompanies");

            migrationBuilder.DropColumn(
                name: "CastMemberId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "CrewMemberId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "CreditsId",
                table: "CrewMembers");

            migrationBuilder.DropColumn(
                name: "CreditsId",
                table: "CastMembers");

            migrationBuilder.AlterColumn<double>(
                name: "Rating",
                table: "Reviews",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Reviews",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CastMemberMovie",
                columns: table => new
                {
                    CastId = table.Column<int>(type: "INTEGER", nullable: false),
                    MoviesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CastMemberMovie", x => new { x.CastId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_CastMemberMovie_CastMembers_CastId",
                        column: x => x.CastId,
                        principalTable: "CastMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CastMemberMovie_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CrewMemberMovie",
                columns: table => new
                {
                    CrewId = table.Column<int>(type: "INTEGER", nullable: false),
                    MoviesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewMemberMovie", x => new { x.CrewId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_CrewMemberMovie_CrewMembers_CrewId",
                        column: x => x.CrewId,
                        principalTable: "CrewMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrewMemberMovie_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieProductionCompany",
                columns: table => new
                {
                    MoviesId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductionCompaniesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieProductionCompany", x => new { x.MoviesId, x.ProductionCompaniesId });
                    table.ForeignKey(
                        name: "FK_MovieProductionCompany_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieProductionCompany_ProductionCompanies_ProductionCompaniesId",
                        column: x => x.ProductionCompaniesId,
                        principalTable: "ProductionCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AuthorId",
                table: "Reviews",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MovieId",
                table: "Reviews",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_CastMemberMovie_MoviesId",
                table: "CastMemberMovie",
                column: "MoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewMemberMovie_MoviesId",
                table: "CrewMemberMovie",
                column: "MoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieProductionCompany_ProductionCompaniesId",
                table: "MovieProductionCompany",
                column: "ProductionCompaniesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_AuthorId",
                table: "Reviews",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_AuthorId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "CastMemberMovie");

            migrationBuilder.DropTable(
                name: "CrewMemberMovie");

            migrationBuilder.DropTable(
                name: "MovieProductionCompany");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_AuthorId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_MovieId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "Reviews",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Reviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Reviews",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "ProductionCompanies",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CastMemberId",
                table: "Movies",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CrewMemberId",
                table: "Movies",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreditsId",
                table: "CrewMembers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreditsId",
                table: "CastMembers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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
                    table.ForeignKey(
                        name: "FK_Credits_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId1",
                table: "Reviews",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionCompanies_MovieId",
                table: "ProductionCompanies",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CastMemberId",
                table: "Movies",
                column: "CastMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CrewMemberId",
                table: "Movies",
                column: "CrewMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewMembers_CreditsId",
                table: "CrewMembers",
                column: "CreditsId");

            migrationBuilder.CreateIndex(
                name: "IX_CastMembers_CreditsId",
                table: "CastMembers",
                column: "CreditsId");

            migrationBuilder.CreateIndex(
                name: "IX_Credits_MovieId",
                table: "Credits",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_CastMembers_Credits_CreditsId",
                table: "CastMembers",
                column: "CreditsId",
                principalTable: "Credits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CrewMembers_Credits_CreditsId",
                table: "CrewMembers",
                column: "CreditsId",
                principalTable: "Credits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_CastMembers_CastMemberId",
                table: "Movies",
                column: "CastMemberId",
                principalTable: "CastMembers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_CrewMembers_CrewMemberId",
                table: "Movies",
                column: "CrewMemberId",
                principalTable: "CrewMembers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionCompanies_Movies_MovieId",
                table: "ProductionCompanies",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId1",
                table: "Reviews",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
