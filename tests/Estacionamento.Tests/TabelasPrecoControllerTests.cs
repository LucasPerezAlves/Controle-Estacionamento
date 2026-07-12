using System;
using Estacionamento.Domain;
using Estacionamento.Web.Controllers;
using Estacionamento.Web.Data;
using FluentAssertions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Estacionamento.Tests;

public class TabelasPrecoControllerTests : TesteComBancoEmMemoria
{
    private readonly TabelaPrecoRepositorio _tabelaPrecoRepositorio;
    private readonly TabelasPrecoController _controller;

    public TabelasPrecoControllerTests()
    {
        _tabelaPrecoRepositorio = new TabelaPrecoRepositorio(Contexto);
        _controller = new TabelasPrecoController(_tabelaPrecoRepositorio);
    }

    [Fact]
    public void Cadastrar_ComDadosValidos_DevePersistirTabelaERedirecionarParaIndex()
    {
        var resultado = _controller.Cadastrar(
            new DateTime(2024, 1, 1),
            new DateTime(2024, 12, 31),
            2.00m,
            1.00m);

        var tabelas = _tabelaPrecoRepositorio.ObterTodas();
        tabelas.Should().ContainSingle(t =>
            t.DataInicioVigencia == new DateTime(2024, 1, 1) &&
            t.DataFimVigencia == new DateTime(2024, 12, 31) &&
            t.ValorHoraInicial == 2.00m &&
            t.ValorHoraAdicional == 1.00m);

        resultado.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    public void Index_QuandoExistemTabelas_DeveListarTodasNoModel()
    {
        var tabela = new TabelaPreco
        {
            DataInicioVigencia = new DateTime(2024, 1, 1),
            DataFimVigencia = new DateTime(2024, 12, 31),
            ValorHoraInicial = 2.00m,
            ValorHoraAdicional = 1.00m
        };
        _tabelaPrecoRepositorio.Adicionar(tabela);
        _tabelaPrecoRepositorio.Salvar();

        var resultado = _controller.Index();

        var viewResult = resultado.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeAssignableTo<IEnumerable<TabelaPreco>>().Subject;
        model.Should().ContainSingle();
    }
}
