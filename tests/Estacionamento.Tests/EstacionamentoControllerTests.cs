using System;
using System.Linq;
using Estacionamento.Domain;
using Estacionamento.Web.Controllers;
using Estacionamento.Web.Data;
using Estacionamento.Web.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
        var registradorDeSaida = new RegistradorDeSaida(_registroRepositorio, _tabelaPrecoRepositorio, new SeletorDeTabelaPreco(), new CalculadoraTarifa());
        _controller = new EstacionamentoController(_registroRepositorio, registradorDeSaida);
    }

    [Fact]
    public void MarcarEntrada_ComPlacaValida_DeveCriarRegistroAbertoERedirecionarParaIndex()
    {
        var resultado = _controller.MarcarEntrada("ABC1234");

        var registro = _registroRepositorio.BuscarAbertoPorPlaca("ABC1234");
        registro.Should().NotBeNull();
        registro!.DataHoraEntrada.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

        resultado.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public void MarcarSaida_QuandoExisteRegistroAbertoETabelaVigente_DeveFecharRegistroERedirecionarParaIndex()
    {
        var dataEntrada = DateTime.Now.AddHours(-1);
        var registro = new RegistroEstacionamento("ABC1234", dataEntrada);
        _registroRepositorio.Adicionar(registro);
        _registroRepositorio.Salvar();

        var tabela = new TabelaPreco
        {
            DataInicioVigencia = dataEntrada.Date.AddYears(-1),
            DataFimVigencia = dataEntrada.Date.AddYears(1),
            ValorHoraInicial = 2.00m,
            ValorHoraAdicional = 1.00m
        };
        _tabelaPrecoRepositorio.Adicionar(tabela);
        _tabelaPrecoRepositorio.Salvar();

        var resultado = _controller.MarcarSaida("ABC1234");

        var registroFechado = Contexto.RegistrosEstacionamento.Single(r => r.Placa == "ABC1234");
        registroFechado.EstaAberto.Should().BeFalse();

        resultado.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public void Index_QuandoExistemRegistros_DeveListarTodosNoModel()
    {
        var registroUm = new RegistroEstacionamento("ABC1234", new DateTime(2024, 5, 10, 8, 0, 0));
        var registroDois = new RegistroEstacionamento("XYZ9999", new DateTime(2024, 5, 11, 9, 0, 0));
        _registroRepositorio.Adicionar(registroUm);
        _registroRepositorio.Adicionar(registroDois);
        _registroRepositorio.Salvar();

        var resultado =  _controller.Index();

        var viewResult = resultado.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeAssignableTo<IEnumerable<RegistroEstacionamento>>().Subject;
        model.Should().HaveCount(2);
    }
}
