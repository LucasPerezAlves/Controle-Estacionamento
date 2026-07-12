using System;
using Estacionamento.Domain;
using Estacionamento.Web.Data;
using FluentAssertions;
using Xunit;

namespace Estacionamento.Tests;

public class RegistroEstacionamentoRepositorioTests : TesteComBancoEmMemoria
{
    [Fact]
    public void BuscarAbertoPorPlaca_QuandoExisteRegistroAbertoComEssaPlaca_DeveRetornarOMesmoRegistro()
    {
        var repositorio = new RegistroEstacionamentoRepositorio(Contexto);
        var registro = new RegistroEstacionamento("ABC1234", new DateTime(2024, 5, 10, 8, 0, 0));

        repositorio.Adicionar(registro);
        repositorio.Salvar();

        var registroEncontrado = repositorio.BuscarAbertoPorPlaca("ABC1234");

        registroEncontrado.Should().NotBeNull();
        registroEncontrado!.Placa.Should().Be("ABC1234");
    }

    [Fact]
    public void ObterTodos_QuandoExistemRegistros_DeveRetornarTodosOrdenadoPorDataEntradaDecrescente()
    {
        var repositorio = new RegistroEstacionamentoRepositorio(Contexto);

        var registroMaisAntigo = new RegistroEstacionamento("ABC1234", new DateTime(2024, 5, 10, 8, 0, 0));
        var registroMaisRecente = new RegistroEstacionamento("XYZ9999", new DateTime(2024, 5, 11, 9, 0, 0));
        repositorio.Adicionar(registroMaisAntigo);
        repositorio.Adicionar(registroMaisRecente);
        repositorio.Salvar();

        var registros = repositorio.ObterTodos().ToList();

        registros.Should().HaveCount(2);
        registros[0].Placa.Should().Be("XYZ9999");
        registros[1].Placa.Should().Be("ABC1234");
    }
}
