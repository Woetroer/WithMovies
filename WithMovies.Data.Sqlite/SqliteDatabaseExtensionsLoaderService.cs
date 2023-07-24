using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WithMovies.Business;
using WithMovies.Domain.Interfaces;

namespace WithMovies.Data.Sqlite;

public class SqliteDatabaseExtensionsLoaderService : IDatabaseExtensionsLoaderService
{
    private IServiceProvider _serviceProvider;

    public SqliteDatabaseExtensionsLoaderService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task EnsureLoaded(string extension)
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        await context.Database.OpenConnectionAsync();

        using var connection = (SqliteConnection)context.Database.GetDbConnection();

        string dir = Directory.GetCurrentDirectory();
        connection.LoadExtension(Path.GetFullPath(Path.Join(dir, "sqlite-extensions", extension)));

        await connection.CloseAsync();
    }
}
