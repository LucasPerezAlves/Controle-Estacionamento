using System;
using Estacionamento.Web.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Estacionamento.Tests;

public abstract class TesteComBancoEmMemoria : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly EstacionamentoDbContext Contexto;

    protected TesteComBancoEmMemoria()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<EstacionamentoDbContext>()
            .UseSqlite(_connection)
            .Options;

        Contexto = new EstacionamentoDbContext(options);
        Contexto.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Contexto.Dispose();
        _connection.Dispose();
    }
}
