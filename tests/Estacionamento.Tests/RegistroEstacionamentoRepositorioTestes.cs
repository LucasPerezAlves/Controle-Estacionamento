using System;
using Estacionamento.Domain;
using Estacionamento.Web.Data;
using FluentAssertions;
using Xunit;

namespace Estacionamento.Tests;

public class RegistroEstacionamentoRepositorioTests : TesteComBancoEmMemoria
{
    [Fact]
    public async Task BuscarAbertoPorPlaca_QuandoExisteRegistroAbertoComEssaPlaca_DeveRetornarOMesmoRegistro()
    {
        var repositorio = new RegistroEstacionamentoRepositorio(Contexto);
        var registro = new RegistroEstacionamento("ABC1234", new DateTime(2024, 5, 10, 8, 0, 0));

        repositorio.Adicionar(registro);
        await repositorio.SalvarAsync();

        var registroEncontrado = await repositorio.BuscarAbertoPorPlacaAsync("ABC1234");

        registroEncontrado.Should().NotBeNull();
        registroEncontrado!.Placa.Should().Be("ABC1234");
    }

    [Fact]
    public async Task ObterHistorico_QuandoExistemRegistros_DeveRetornarOrdenadoPorDataEntradaDecrescente()
    {
        var repositorio = new RegistroEstacionamentoRepositorio(Contexto);

        var registroMaisAntigo = new RegistroEstacionamento("ABC1234", new DateTime(2024, 5, 10, 8, 0, 0));
        var registroMaisRecente = new RegistroEstacionamento("XYZ9999", new DateTime(2024, 5, 11, 9, 0, 0));
        repositorio.Adicionar(registroMaisAntigo);
        repositorio.Adicionar(registroMaisRecente);
        await repositorio.SalvarAsync();

        var registros = (await repositorio.ObterHistoricoAsync(pagina: 1, tamanhoPagina: 10)).ToList();

        registros.Should().HaveCount(2);
        registros[0].Placa.Should().Be("XYZ9999");
        registros[1].Placa.Should().Be("ABC1234");
    }

    [Fact]
    public async Task ObterHistorico_ComTamanhoDePaginaMenorQueOTotal_DeveRetornarApenasAPagina()
    {
        var repositorio = new RegistroEstacionamentoRepositorio(Contexto);

        for (var i = 0; i < 5; i++)
        {
            repositorio.Adicionar(new RegistroEstacionamento($"PLACA{i:D2}", new DateTime(2024, 1, 1).AddDays(i)));
        }
        await repositorio.SalvarAsync();

        var primeiraPagina = (await repositorio.ObterHistoricoAsync(pagina: 1, tamanhoPagina: 2)).ToList();
        var segundaPagina = (await repositorio.ObterHistoricoAsync(pagina: 2, tamanhoPagina: 2)).ToList();

        primeiraPagina.Should().HaveCount(2);
        segundaPagina.Should().HaveCount(2);
        primeiraPagina.Should().NotContain(segundaPagina);
    }

    [Fact]
    public async Task ObterAbertos_QuandoExisteRegistroFechado_NaoDeveRetornarEle()
    {
        var repositorio = new RegistroEstacionamentoRepositorio(Contexto);

        var registroAberto = new RegistroEstacionamento("ABC1234", new DateTime(2024, 5, 10, 8, 0, 0));
        var registroFechado = new RegistroEstacionamento("XYZ9999", new DateTime(2024, 5, 10, 8, 0, 0));
        registroFechado.RegistrarSaida(new DateTime(2024, 5, 10, 9, 0, 0), 2.00m);
        repositorio.Adicionar(registroAberto);
        repositorio.Adicionar(registroFechado);
        await repositorio.SalvarAsync();

        var abertos = await repositorio.ObterAbertosAsync();

        abertos.Should().ContainSingle(r => r.Placa == "ABC1234");
    }
}
