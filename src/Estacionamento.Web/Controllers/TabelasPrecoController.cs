using Estacionamento.Domain;
using Estacionamento.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace Estacionamento.Web.Controllers;

public class TabelasPrecoController : Controller
{
    private readonly ITabelaPrecoRepositorio _tabelaPrecoRepositorio;
    private readonly ILogger<TabelasPrecoController> _logger;

    public TabelasPrecoController(ITabelaPrecoRepositorio tabelaPrecoRepositorio, ILogger<TabelasPrecoController> logger)
    {
        _tabelaPrecoRepositorio = tabelaPrecoRepositorio;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var tabelas = await _tabelaPrecoRepositorio.ObterTodasAsync();
        return View(tabelas);
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar(DateTime dataInicioVigencia, DateTime dataFimVigencia, decimal valorHoraInicial, decimal valorHoraAdicional)
    {
        var tabelas = await _tabelaPrecoRepositorio.ObterTodasAsync();
        var existeSobreposicao = tabelas.Any(t => dataInicioVigencia <= t.DataFimVigencia && dataFimVigencia >= t.DataInicioVigencia);

        if (existeSobreposicao)
        {
            _logger.LogWarning("Tentativa de cadastrar tabela de preco com vigencia sobreposta ({Inicio} a {Fim}).", dataInicioVigencia, dataFimVigencia);
            TempData["Erro"] = "Ja existe uma tabela de preco vigente nesse periodo.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var tabela = new TabelaPreco(dataInicioVigencia, dataFimVigencia, valorHoraInicial, valorHoraAdicional);
            _tabelaPrecoRepositorio.Adicionar(tabela);
            await _tabelaPrecoRepositorio.SalvarAsync();

            _logger.LogInformation("Tabela de preco cadastrada para o periodo de {Inicio} a {Fim}.", dataInicioVigencia, dataFimVigencia);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Falha ao cadastrar tabela de preco.");
            TempData["Erro"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
