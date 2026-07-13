using System;
using System.Collections.Generic;
using System.Linq;
using Estacionamento.Domain;
using Estacionamento.Web.Controllers;
using Estacionamento.Web.Data;
using Estacionamento.Web.Models;
using Estacionamento.Web.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Estacionamento.Tests;

public class EstacionamentoControllerTests : TesteComBancoEmMemoria
{
    private readonly RegistroEstacionamentoRepositorio _registroRepositorio;
    private readonly TabelaPrecoRepositorio _tabelaPrecoRepositorio;
    private readonly EstacionamentoController _controller;

    public EstacionamentoControllerTests()
    {
        _registroRepositorio = new RegistroEstacionamentoRepositorio(Contexto);
        _tabelaPrecoRepositorio = new TabelaPrecoRepositorio(Contexto);
        var registradorDeEntrada = new RegistradorDeEntrada(_registroRepositorio);
        var registradorDeSaida = new RegistradorDeSaida(_registroRepositorio, _tabelaPrecoRepositorio, new SeletorDeTabelaPreco(), new CalculadoraTarifa());
        _controller = new EstacionamentoController(_registroRepositorio, registradorDeEntrada, registradorDeSaida, NullLogger<EstacionamentoController>.Instance)
        {
            TempData = new TempDataDictionary(new DefaultHttpContext(), new TempDataProviderFake())
        };
    }

    private class TempDataProviderFake : ITempDataProvider
    {
        public IDictionary<string, object> LoadTempData(HttpContext context) => new Dictionary<string, object>();

        public void SaveTempData(HttpContext context, IDictionary<string, object> values) { }
    }

    [Fact]
    public async Task MarcarEntrada_ComPlacaEmBranco_NaoDeveCriarRegistro()
    {
        await _controller.MarcarEntrada("   ");

        (await _registroRepositorio.ObterAbertosAsync()).Should().BeEmpty();
    }

    [Fact]
    public async Task MarcarEntrada_ComPlacaValida_DeveCriarRegistroAbertoERedirecionarParaIndex()
    {
        var resultado = await _controller.MarcarEntrada("ABC1234");

        var registro = await _registroRepositorio.BuscarAbertoPorPlacaAsync("ABC1234");
        registro.Should().NotBeNull();
        registro!.DataHoraEntrada.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

        resultado.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public async Task MarcarEntrada_QuandoJaExisteRegistroAbertoParaAPlaca_NaoDeveCriarNovoRegistro()
    {
        await _controller.MarcarEntrada("ABC1234");

        await _controller.MarcarEntrada("ABC1234");

        var registros = await _registroRepositorio.ObterAbertosAsync();
        registros.Should().ContainSingle(r => r.Placa == "ABC1234");
    }

    [Fact]
    public async Task MarcarSaida_QuandoExisteRegistroAbertoETabelaVigente_DeveFecharRegistroERedirecionarParaIndex()
    {
        var dataEntrada = DateTime.Now.AddHours(-1);
        var registro = new RegistroEstacionamento("ABC1234", dataEntrada);
        _registroRepositorio.Adicionar(registro);
        await _registroRepositorio.SalvarAsync();

        var tabela = new TabelaPreco(dataEntrada.Date.AddYears(-1), dataEntrada.Date.AddYears(1), 2.00m, 1.00m);
        _tabelaPrecoRepositorio.Adicionar(tabela);
        await _tabelaPrecoRepositorio.SalvarAsync();

        var resultado = await _controller.MarcarSaida("ABC1234");

        var registroFechado = Contexto.RegistrosEstacionamento.Single(r => r.Placa == "ABC1234");
        registroFechado.EstaAberto.Should().BeFalse();

        resultado.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public async Task Index_QuandoExistemRegistros_DeveListarTodosNoModel()
    {
        var registroUm = new RegistroEstacionamento("ABC1234", new DateTime(2024, 5, 10, 8, 0, 0));
        var registroDois = new RegistroEstacionamento("XYZ9999", new DateTime(2024, 5, 11, 9, 0, 0));
        _registroRepositorio.Adicionar(registroUm);
        _registroRepositorio.Adicionar(registroDois);
        await _registroRepositorio.SalvarAsync();

        var resultado = await _controller.Index();

        var viewResult = resultado.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<EstacionamentoIndexViewModel>().Subject;
        model.Abertos.Should().HaveCount(2);
        model.Historico.Should().HaveCount(2);
        model.PaginaAtual.Should().Be(1);
        model.TotalPaginas.Should().Be(1);
    }

    [Fact]
    public async Task Index_ComMaisRegistrosQueOTamanhoDaPagina_DevePaginarOHistorico()
    {
        for (var i = 0; i < 15; i++)
        {
            _registroRepositorio.Adicionar(new RegistroEstacionamento($"PLACA{i:D2}", new DateTime(2024, 1, 1).AddDays(i)));
        }
        await _registroRepositorio.SalvarAsync();

        var primeiraPagina = await _controller.Index();
        var primeiraPaginaModel = ((ViewResult)primeiraPagina).Model.Should().BeOfType<EstacionamentoIndexViewModel>().Subject;
        primeiraPaginaModel.Historico.Should().HaveCount(10);
        primeiraPaginaModel.TotalPaginas.Should().Be(2);

        var segundaPagina = await _controller.Index(pagina: 2);
        var segundaPaginaModel = ((ViewResult)segundaPagina).Model.Should().BeOfType<EstacionamentoIndexViewModel>().Subject;
        segundaPaginaModel.Historico.Should().HaveCount(5);
    }
}
