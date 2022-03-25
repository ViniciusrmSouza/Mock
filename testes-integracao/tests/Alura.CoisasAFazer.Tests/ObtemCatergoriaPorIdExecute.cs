using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Moq;
using Xunit;

namespace Alura.CoisasAFazer.Tests
{
    public class ObtemCatergoriaPorIdExecute
    {
        [Fact]
        public void ObtemCategoria_IdExistente_Sucesso()
        {
            //Arrange
            var categoriaId = 1;
            var comando = new ObtemCategoriaPorId(categoriaId);

            var mock = new Mock<IRepositorioTarefas>();
            mock.Setup(r => r.ObtemCategoriaPorId(It.Is<int>(x=> x == 1)))
                .Returns(new Categoria(1,"teste"));
            
            var repo = mock.Object;
            var handler = new ObtemCategoriaPorIdHandler(repo);
            
            //Act
            var result = handler.Execute(comando);
            
            //Assert
            mock.Verify(r => r.ObtemCategoriaPorId(categoriaId), 
                Times.Once);
            Assert.Equal(categoriaId, result.Id);
        }
    }
}