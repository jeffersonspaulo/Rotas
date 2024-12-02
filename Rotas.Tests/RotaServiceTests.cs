using Moq;
using Xunit;
using Rotas.API.Data.Interfaces;
using Rotas.API.Entities;
using Rotas.API.Services;
using FluentAssertions;
using Rotas.API.Models;

namespace Rotas.Tests
{
    public class RotaServiceTests
    {
        private readonly Mock<IRepository<Rota>> _repositoryMock;
        private readonly RotaService _rotaService;

        public RotaServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Rota>>();
            _rotaService = new RotaService(_repositoryMock.Object);
        }

        [Fact]
        public async Task ObterMelhorRotaAsync_DeveRetornarRotaCorreta_QuandoOrigemEDestinoSaoGRU_CDG()
        {
            // Arrange
            var rotas = new List<Rota>
            {
                new Rota { Origem = "GRU", Destino = "BRC", Valor = 10 },
                new Rota { Origem = "BRC", Destino = "SCL", Valor = 5 },
                new Rota { Origem = "GRU", Destino = "CDG", Valor = 75 },
                new Rota { Origem = "GRU", Destino = "SCL", Valor = 20 },
                new Rota { Origem = "GRU", Destino = "ORL", Valor = 56 },
                new Rota { Origem = "ORL", Destino = "CDG", Valor = 5 },
                new Rota { Origem = "SCL", Destino = "ORL", Valor = 20 }
            };

            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(rotas);

            // Act
            var resultado = await _rotaService.ObterMelhorRotaAsync("GRU", "CDG");

            // Assert
            resultado.Should().Be("GRU - BRC - SCL - ORL - CDG ao custo de $40");
        }

        [Fact]
        public async Task AdicionarAsync_DeveAdicionarRota()
        {
            var rotaModel = new RotaModel
            {
                Origem = "GRU",
                Destino = "BRC",
                Valor = 10
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Rota>()))
                .ReturnsAsync(new Rota
                {
                    Id = 1,
                    Origem = rotaModel.Origem,
                    Destino = rotaModel.Destino,
                    Valor = rotaModel.Valor
                });

            var rota = await _rotaService.AdicionarAsync(rotaModel);

            rota.Id.Should().Be(1);
            rota.Origem.Should().Be("GRU");
            rota.Destino.Should().Be("BRC");
            rota.Valor.Should().Be(10);
        }

    }
}