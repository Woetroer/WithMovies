using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WithMovies.Business;
using WithMovies.Domain.Interfaces;

namespace WithMovies.Data.Sqlite;

public static class DataContextExtensions
{
    public static DataContext LoadExtension(this DataContext dataContext, string extension)
    {
        using var connection = (SqliteConnection)dataContext.Database.GetDbConnection();

        string dir = Directory.GetCurrentDirectory();
        connection.LoadExtension(Path.GetFullPath(Path.Join(dir, "sqlite-extensions", extension)));

        return dataContext;
    }
}
