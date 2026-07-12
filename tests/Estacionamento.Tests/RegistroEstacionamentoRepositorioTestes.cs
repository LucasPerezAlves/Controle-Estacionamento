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
}
