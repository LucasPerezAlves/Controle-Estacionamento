using System;
using Estacionamento.Domain;
using FluentAssertions;
using Xunit;

namespace Estacionamento.Tests;

public class TabelaPrecoTests
{
    [Fact]
    public void Criar_ComDadosValidos_DevePreencherPropriedadesCorretamente()
    {
        var tabela = new TabelaPreco(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31), 2.00m, 1.00m);

        tabela.DataInicioVigencia.Should().Be(new DateTime(2024, 1, 1));
        tabela.DataFimVigencia.Should().Be(new DateTime(2024, 12, 31));
        tabela.ValorHoraInicial.Should().Be(2.00m);
        tabela.ValorHoraAdicional.Should().Be(1.00m);
    }

    [Fact]
    public void Criar_ComDataFimAnteriorADataInicio_DeveLancarExcecao()
    {
        var acao = () => new TabelaPreco(new DateTime(2024, 12, 31), new DateTime(2024, 1, 1), 2.00m, 1.00m);

        acao.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Criar_ComValorHoraInicialNegativo_DeveLancarExcecao()
    {
        var acao = () => new TabelaPreco(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31), -1.00m, 1.00m);

        acao.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Criar_ComValorHoraAdicionalNegativo_DeveLancarExcecao()
    {
        var acao = () => new TabelaPreco(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31), 2.00m, -1.00m);

        acao.Should().Throw<ArgumentException>();
    }
}
