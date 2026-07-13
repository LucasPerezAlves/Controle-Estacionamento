using Estacionamento.Domain;

namespace Estacionamento.Web.Data;

public interface ITabelaPrecoRepositorio
{
    void Adicionar(TabelaPreco tabela);
    Task<IEnumerable<TabelaPreco>> ObterTodasAsync();
    Task SalvarAsync();
}
