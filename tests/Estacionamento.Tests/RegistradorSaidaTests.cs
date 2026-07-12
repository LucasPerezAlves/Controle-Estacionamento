using System;
using System.Linq;
using Estacionamento.Domain;
using Estacionamento.Web.Data;
using Estacionamento.Web.Services;
using FluentAssertions;
using Xunit;

namespace Estacionamento.Tests;

public class RegistradorDeSaidaTests : TesteComBancoEmMemoria
{
    [Fact]
    public void Registrar_QuandoExisteRegistroAbertoETabelaVigente_DeveCalcularECommRegistrarValorPago()
    {
        var registroRepositorio = new RegistroEstacionamentoRepositorio(Contexto);
        var tabelaPrecoRepositorio = new TabelaPrecoRepositorio(Contexto);

        var dataEntrada = new DateTime(2024, 5, 10, 8, 0, 0);
        var registro = new RegistroEstacionamento("ABC1234", dataEntrada);
        registroRepositorio.Adicionar(registro);
        registroRepositorio.Salvar();

        var tabela = new TabelaPreco
        {
            DataInicioVigencia = new DateTime(2024, 1, 1),
            DataFimVigencia = new DateTime(2024, 12, 31),
            ValorHoraInicial = 2.00m,
            ValorHoraAdicional = 1.00m
        };
        tabelaPrecoRepositorio.Adicionar(tabela);
        tabelaPrecoRepositorio.Salvar();

        var registrador = new RegistradorDeSaida(
            registroRepositorio,
            tabelaPrecoRepositorio,
            new SeletorDeTabelaPreco(),
            new CalculadoraTarifa()
        );

        var dataSaida = new DateTime(2024, 5, 10, 9, 0, 0);
        registrador.Registrar("ABC1234", dataSaida);

        var registroPersistido = Contexto.RegistrosEstacionamento.Single(r => r.Placa == "ABC1234");

        registroPersistido.EstaAberto.Should().BeFalse();
        registroPersistido.ValorPago.Should().Be(2.00m);
    }

    [Fact]
    public void Registrar_QuandoNaoExisteRegistroAbertoParaAPlaca_DeveLancarExcecao()
    {
        var registroRepositorio = new RegistroEstacionamentoRepositorio(Contexto);
        var tabelaPrecoRepositorio = new TabelaPrecoRepositorio(Contexto);

        var registrador = new RegistradorDeSaida(
            registroRepositorio,
            tabelaPrecoRepositorio,
            new SeletorDeTabelaPreco(),
            new CalculadoraTarifa());

        var acao = () => registrador.Registrar("XYZ9999", new DateTime(2024, 5, 10, 9, 0, 0));

        acao.Should().Throw<InvalidOperationException>();
    }

}