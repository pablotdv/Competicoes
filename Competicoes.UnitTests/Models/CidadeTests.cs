using Competicoes.Models;
using Competicoes.UnitTests.Infraestrutura;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Competicoes.UnitTests.Models
{
    public class CidadeTests
    {
        [Fact]
        public void Cidade_Nome_Obrigatorio()
        {
            var cidade = new Cidade()
            {
                CidadeId = Guid.NewGuid(),
                Nome = "",
            };

            var result = cidade.ValidateModel();

            Assert.Single(result, x => x.ErrorMessage.Equals("O campo Cidade é obrigatório"));
        }

        [Fact]
        public void Cidade_Nome_Maximo()
        {
            var cidade = new Cidade()
            {
                CidadeId = Guid.NewGuid(),
                Nome = new string('*', 201),
            };

            var result = cidade.ValidateModel();

            Assert.Single(result, x => x.ErrorMessage.Equals("O campo Cidade deve ser uma string com um comprimento máximo de 200"));
        }

        [Fact]
        public void Cidade_Id_Vazio()
        {
            var cidade = new Cidade()
            {
                Nome = "Santa Maria",
            };

            Assert.Equal(Guid.Empty, cidade.CidadeId);
        }
    }
}
