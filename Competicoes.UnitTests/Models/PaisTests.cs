using Competicoes.Models;
using Competicoes.UnitTests.Infraestrutura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Xunit;

namespace Competicoes.UnitTests.Models
{
    public class PaisTests
    {
        [Fact]
        public void Pais_Nome_Obrigatorio()
        {
            var pais = new Pais()
            {
                PaisId = Guid.NewGuid(),
                Nome = "",
                Sigla = "BR",
            };

            var result = pais.ValidateModel();

            Assert.True(result.Count == 1);
            Assert.Equal("O campo País é obrigatório", result.First().ErrorMessage);
        }

        [Fact]
        public void Pais_Nome_Maximo()
        {
            var pais = new Pais()
            {
                PaisId = Guid.NewGuid(),
                Nome = new string('*', 201),
                Sigla = "BR",
            };

            var result = pais.ValidateModel();
            Assert.True(result.Count == 1);
            Assert.Equal("O campo País deve ser uma string com um comprimento máximo de 200", result.First().ErrorMessage);

        }

        [Fact]
        public void Pais_Sigla_Obrigatorio()
        {
            var pais = new Pais()
            {
                PaisId = Guid.NewGuid(),
                Nome = "Brasil",
                Sigla = "",
            };

            var result = pais.ValidateModel();

            Assert.True(result.Count == 1);
            Assert.Equal("O campo Sigla é obrigatório", result.First().ErrorMessage);
        }

        [Fact]
        public void Pais_Sigla_Maximo()
        {
            var pais = new Pais() {
                PaisId = Guid.NewGuid(),
                Nome = "Brasil",
                Sigla = new string('*', 4),
            };

            var result = pais.ValidateModel();

            Assert.True(result.Count == 1);
            Assert.Equal("O campo Sigla deve ser uma string com um comprimento máximo de 3", result.First().ErrorMessage);
        }

        [Fact]
        public void Pais_Id_Vazio()
        {
            var pais = new Pais()
            {
                Nome = "Brasil",
                Sigla = "BR"
            };

            Assert.Equal(Guid.Empty, pais.PaisId);
        }
    }
}
