using System;
using Estacionamento.Web.Data;
using Estacionamento.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Estacionamento.Web.Controllers;

public class TabelasPrecoController : Controller
{
    private readonly TabelaPrecoRepositorio _tabelaPrecoRepositorio;

    public TabelasPrecoController(TabelaPrecoRepositorio tabelaPrecoRepositorio)
    {
        _tabelaPrecoRepositorio = tabelaPrecoRepositorio;
    }

    public IActionResult Index()
    {
        var tabelas = _tabelaPrecoRepositorio.ObterTodas();
        return View(tabelas);
    }

    [HttpPost]
    public IActionResult Cadastrar(DateTime dataInicioVigencia, DateTime dataFimVigencia, decimal valorHoraInicial, decimal valorHoraAdicional)
    {
        var tabela = new TabelaPreco
        {
            DataInicioVigencia = dataInicioVigencia,
            DataFimVigencia = dataFimVigencia,
            ValorHoraInicial = valorHoraInicial,
            ValorHoraAdicional = valorHoraAdicional
        };

        _tabelaPrecoRepositorio.Adicionar(tabela);
        _tabelaPrecoRepositorio.Salvar();

        return RedirectToAction(nameof(Index));
    }

}
