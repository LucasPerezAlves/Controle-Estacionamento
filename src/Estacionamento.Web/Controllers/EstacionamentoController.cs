using Estacionamento.Domain;
using Estacionamento.Web.Data;
using Estacionamento.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Estacionamento.Web.Controllers;

public class EstacionamentoController : Controller
{
    private readonly IRegistroEstacionamentoRepositorio _registroRepositorio;
    private readonly RegistradorDeSaida _registradorDeSaida;
    private readonly ILogger<EstacionamentoController> _logger;

    public EstacionamentoController(
        IRegistroEstacionamentoRepositorio registroRepositorio,
        RegistradorDeSaida registradorDeSaida,
        ILogger<EstacionamentoController> logger)
    {
        _registroRepositorio = registroRepositorio;
        _registradorDeSaida = registradorDeSaida;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var registros = await _registroRepositorio.ObterTodosAsync();
        return View(registros);
    }

    [HttpPost]
    public async Task<IActionResult> MarcarEntrada(string placa)
    {
        if (string.IsNullOrWhiteSpace(placa))
        {
            TempData["Erro"] = "Informe uma placa valida.";
            return RedirectToAction(nameof(Index));
        }

        if (await _registroRepositorio.BuscarAbertoPorPlacaAsync(placa) is not null)
        {
            _logger.LogWarning("Tentativa de marcar entrada para a placa {Placa}, que ja possui registro aberto.", placa);
            TempData["Erro"] = $"Ja existe um registro aberto para a placa {placa}.";
            return RedirectToAction(nameof(Index));
        }

        var registro = new RegistroEstacionamento(placa, DateTime.Now);
        _registroRepositorio.Adicionar(registro);
        await _registroRepositorio.SalvarAsync();

        _logger.LogInformation("Entrada registrada para a placa {Placa}.", placa);

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
