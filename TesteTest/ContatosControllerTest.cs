using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteApi.Controllers;
using TesteMed.Models;
using Xunit;

namespace TesteTest
{
    public class ContatosControllerTest
    {
        private readonly Mock<DbSet<Contato>> _mockSet;
        private readonly Mock<Context> _mockContext;
        private readonly Contato _contato;

        public ContatosControllerTest()
        {
            _mockSet = new Mock<DbSet<Contato>>();
            _mockContext = new Mock<Context>();
            _contato = new Contato { Id = 1, Nome = "Teste Contato", DataNasc = new DateTime(2002, 10, 16), Sexo = sexo.Masculino, Ativo = true};

            _mockContext.Setup(m => m.Contatos).Returns(_mockSet.Object);

            _mockContext.Setup(m => m.Contatos.FindAsync(1))
                .ReturnsAsync(_contato);

            _mockContext.Setup(m => m.SetModified(_contato));

            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
        }

        [Fact]
        public async Task Get_Contato()
        {
            var service = new ContatosController(_mockContext.Object);

            await service.GetContato(1);

            _mockSet.Verify(m => m.FindAsync(1), Times.Once()); 
;
        }

        [Fact]
        public async Task Put_Contato()
        {
            var service = new ContatosController(_mockContext.Object);

            await service.PutContato(1, _contato);

            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Post_Contato()
        {
            var service = new ContatosController(_mockContext.Object);
            await service.PostContato(_contato);

            _mockSet.Verify(x => x.Add(_contato), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public async Task Delete_Contato()
        {
            var service = new ContatosController(_mockContext.Object);
            await service.DeleteContato(1);

            _mockSet.Verify(m => m.FindAsync(1),
                Times.Once());
            _mockSet.Verify(x => x.Remove(_contato), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once());
        }
    }
}
