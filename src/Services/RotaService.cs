using Rotas.API.Data.Interfaces;
using Rotas.API.Entities;
using Rotas.API.Models;
using Rotas.API.Services.Interfaces;

namespace Rotas.API.Services
{
    public class RotaService : IRotaService
    {
        private readonly IRepository<Rota> _repository;

        public RotaService(IRepository<Rota> repository)
        {
            _repository = repository;
        }

        public async Task<string> ObterMelhorRotaAsync(string origem, string destino)
        {
            var rotas = await _repository.GetAllAsync();
            var grafo = ConstruirGrafo(rotas);
            var (melhorCaminho, custoTotal) = CalcularMelhorRota(grafo, origem, destino);

            if (melhorCaminho == null || !melhorCaminho.Any())
                throw new Exception($"Nenhuma rota disponível entre {origem} e {destino}.");

            return $"{string.Join(" - ", melhorCaminho)} ao custo de ${custoTotal}";
        }

        public async Task<IEnumerable<Rota>> ObterTodasAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Rota> AdicionarAsync(RotaModel rotaModel)
        {
            var rota = new Rota
            {
                Origem = rotaModel.Origem,
                Destino = rotaModel.Destino,
                Valor = rotaModel.Valor
            };
            
            return await _repository.AddAsync(rota);
        }

        public async Task AtualizarAsync(int id, RotaModel rotaModel)
        {
            var rota = await _repository.GetByIdAsync(id);

            if (rota == null)
                throw new Exception($"Nenhuma rota encontrada com o id {id}.");

            rota.Origem = rotaModel.Origem;
            rota.Destino = rotaModel.Destino;
            rota.Valor = rotaModel.Valor;

            await _repository.UpdateAsync(rota);
        }

        public async Task RemoverAsync(int id)
        {
            var rota = await _repository.GetByIdAsync(id);

            if (rota == null)
                throw new Exception($"Nenhuma rota encontrada com o id {id}.");

            await _repository.DeleteAsync(rota.Id);
        }

        private Dictionary<string, List<(string destino, decimal custo)>> ConstruirGrafo(IEnumerable<Rota> rotas)
        {
            var grafo = new Dictionary<string, List<(string destino, decimal custo)>>();

            foreach (var rota in rotas)
            {
                if (!grafo.ContainsKey(rota.Origem))
                    grafo[rota.Origem] = new List<(string destino, decimal custo)>();

                grafo[rota.Origem].Add((rota.Destino, rota.Valor));

                if (!grafo.ContainsKey(rota.Destino))
                    grafo[rota.Destino] = new List<(string destino, decimal custo)>();
            }

            return grafo;
        }

        private (List<string> caminho, decimal custo) CalcularMelhorRota(Dictionary<string, List<(string destino, decimal custo)>> grafo, string origem, string destino)
        {
            var distancias = new Dictionary<string, decimal>();
            var anteriores = new Dictionary<string, string>();
            var visitados = new HashSet<string>();
            var fila = new PriorityQueue<string, decimal>();

            foreach (var vertice in grafo.Keys)
            {
                distancias[vertice] = decimal.MaxValue;
                anteriores[vertice] = null;
            }

            distancias[origem] = 0;

            fila.Enqueue(origem, 0);

            while (fila.Count > 0)
            {
                var atual = fila.Dequeue();
                if (visitados.Contains(atual)) continue;

                visitados.Add(atual);

                if (atual == destino)
                    break;

                if (!grafo.ContainsKey(atual)) continue;

                foreach (var (proximo, custo) in grafo[atual])
                {
                    var novaDistancia = distancias[atual] + custo;
                    if (novaDistancia < distancias[proximo])
                    {
                        distancias[proximo] = novaDistancia;
                        anteriores[proximo] = atual;
                        fila.Enqueue(proximo, novaDistancia);
                    }
                }
            }

            var caminho = new List<string>();
            var atualCaminho = destino;

            while (atualCaminho != null)
            {
                caminho.Add(atualCaminho);
                atualCaminho = anteriores[atualCaminho];
            }

            caminho.Reverse();

            return (caminho, distancias[destino]);
        }

    }
}
