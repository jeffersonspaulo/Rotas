using System.ComponentModel.DataAnnotations;

namespace Rotas.API.Models
{
    public class RotaModel
    {
        [Required]
        public required string Origem { get; set; }

        [Required]
        public required string Destino { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal Valor { get; set; }
    }
}
