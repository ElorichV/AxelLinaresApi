using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using AxelLinaresApi.Data;

namespace AxelLinaresApi.Tests.Fixtures
{
    /// <summary>
    /// Fixture aislada con SQLite In-Memory que garantiza que ningún test toque archivos físicos ni red externa.
    /// </summary>
    public sealed class SqliteInMemoryDbFixture : IDisposable
    {
        private readonly SqliteConnection _connection;
        public ProfileDbContext Context { get; }

        public SqliteInMemoryDbFixture()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<ProfileDbContext>()
                .UseSqlite(_connection)
                .Options;

            Context = new ProfileDbContext(options);
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }
    }
}
