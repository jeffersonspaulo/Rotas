using Rotas.API.Entities;
using Rotas.API.Models;

namespace Rotas.API.Services.Interfaces
{
    public interface IRotaService
    {
        Task<string> ObterMelhorRotaAsync(string origem, string destino);
        Task<IEnumerable<Rota>> ObterTodasAsync();
        Task<Rota> AdicionarAsync(RotaModel rotaModel);
        Task AtualizarAsync(int id, RotaModel rotaModel);
        Task RemoverAsync(int id);
    }
}
