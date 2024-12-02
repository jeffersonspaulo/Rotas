using System.ComponentModel.DataAnnotations;

namespace Rotas.API.Entities
{
    public class Rota
    {
        [Key]
        public int Id { get; set; }

        public required string Origem { get; set; }

        public required string Destino { get; set; }

        public decimal Valor { get; set; }
    }
}
