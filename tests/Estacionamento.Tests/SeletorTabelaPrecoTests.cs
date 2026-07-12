using System;
using System.Collections.Generic;
using Estacionamento.Domain;
using FluentAssertions;
using Xunit;

namespace Estacionamento.Tests;

public class SeletorDeTabelaPrecoTests
{
    [Fact]
    public void Selecionar_QuandoDataEstaDentroDaVigencia_DeveRetornarTabelaCorrespondente()
    {
        var tabela = new TabelaPreco
        {
            DataInicioVigencia = new DateTime(2024, 1, 1),
            DataFimVigencia = new DateTime(2024, 12, 31),
            ValorHoraInicial = 2.00m,
            ValorHoraAdicional = 1.00m
        };

        var tabelas = new List<TabelaPreco> { tabela };

        var seletor = new SeletorDeTabelaPreco();

        var tabelaSelecionada = seletor.Selecionar(tabelas, new DateTime(2024, 6, 15));

        tabelaSelecionada.Should().Be(tabela);
    }

    [Fact]
    public void Selecionar_QuandoNenhumaTabelaCobreAData_DeveLancarExcecao()
    {
        var tabela = new TabelaPreco
        {
            DataInicioVigencia = new DateTime(2024, 1, 1),
            DataFimVigencia = new DateTime(2024, 12, 31),
            ValorHoraInicial = 2.00m,
            ValorHoraAdicional = 1.00m
        };

        var tabelas = new List<TabelaPreco> { tabela };

        var seletor = new SeletorDeTabelaPreco();

        var acao = () => seletor.Selecionar(tabelas, new DateTime(2025, 1, 1));

        acao.Should().Throw<InvalidOperationException>();
    }
}
