using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WithMovies.Business.Migrations
{
    /// <inheritdoc />
    public partial class SqliteDataContext2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "KeywordRecords",
                newName: "w");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "KeywordRecords",
                newName: "n");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "w",
                table: "KeywordRecords",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "n",
                table: "KeywordRecords",
                newName: "Name");
        }
    }
}
