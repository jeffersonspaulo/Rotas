using Microsoft.AspNetCore.Mvc;
using Rotas.API.Models;
using Rotas.API.Services.Interfaces;
using System.Net;

namespace Rotas.API.Controllers
{
    [ApiController]
    [Route("rotas")]
    public class RotasController : ControllerBase
    {
        private readonly IRotaService _rotaService;
        private readonly ILogger<RotasController> _logger;

        public RotasController(IRotaService rotaService, ILogger<RotasController> logger)
        {
            _rotaService = rotaService;
            _logger = logger;
        }

        [HttpGet("melhor-rota/{origem}/{destino}")]
        public async Task<IActionResult> ObterMelhorRota(string origem, string destino)
        {
            try
            {
                var melhorRota = await _rotaService.ObterMelhorRotaAsync(origem, destino);

                return Ok(melhorRota);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodasRotas()
        {
            var rotas = await _rotaService.ObterTodasAsync();

            return Ok(rotas);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] RotaModel rotaModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var rotaCriada = await _rotaService.AdicionarAsync(rotaModel);

                return Ok(rotaCriada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] RotaModel rotaModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _rotaService.AtualizarAsync(id, rotaModel);

                return Ok();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                await _rotaService.RemoverAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
