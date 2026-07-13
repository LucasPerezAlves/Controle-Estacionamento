using Estacionamento.Web.Data;
using Estacionamento.Web.Models;
using Estacionamento.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Estacionamento.Web.Controllers;

public class EstacionamentoController : Controller
{
    private const int TamanhoPaginaHistorico = 10;

    private readonly IRegistroEstacionamentoRepositorio _registroRepositorio;
    private readonly RegistradorDeEntrada _registradorDeEntrada;
    private readonly RegistradorDeSaida _registradorDeSaida;
    private readonly ILogger<EstacionamentoController> _logger;

    public EstacionamentoController(
        IRegistroEstacionamentoRepositorio registroRepositorio,
        RegistradorDeEntrada registradorDeEntrada,
        RegistradorDeSaida registradorDeSaida,
        ILogger<EstacionamentoController> logger)
    {
        _registroRepositorio = registroRepositorio;
        _registradorDeEntrada = registradorDeEntrada;
        _registradorDeSaida = registradorDeSaida;
        _logger = logger;
    }

    public async Task<IActionResult> Index(int pagina = 1)
    {
        if (pagina < 1)
        {
            pagina = 1;
        }

        var abertos = await _registroRepositorio.ObterAbertosAsync();
        var historico = await _registroRepositorio.ObterHistoricoAsync(pagina, TamanhoPaginaHistorico);
        var totalRegistros = await _registroRepositorio.ContarTodosAsync();
        var totalPaginas = Math.Max(1, (int)Math.Ceiling(totalRegistros / (double)TamanhoPaginaHistorico));

        var viewModel = new EstacionamentoIndexViewModel
        {
            Abertos = abertos.ToList(),
            Historico = historico.ToList(),
            PaginaAtual = pagina,
            TotalPaginas = totalPaginas
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> MarcarEntrada(string placa)
    {
        try
        {
            await _registradorDeEntrada.RegistrarAsync(placa, DateTime.Now);
            _logger.LogInformation("Entrada registrada para a placa {Placa}.", placa);
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            _logger.LogWarning(ex, "Falha ao marcar entrada para a placa {Placa}.", placa);
            TempData["Erro"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> MarcarSaida(string placa)
    {
        try
        {
            await _registradorDeSaida.RegistrarAsync(placa, DateTime.Now);
            _logger.LogInformation("Saida registrada para a placa {Placa}.", placa);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Falha ao marcar saida para a placa {Placa}.", placa);
            TempData["Erro"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
