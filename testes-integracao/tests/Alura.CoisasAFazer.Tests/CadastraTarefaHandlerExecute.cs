using System;
using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Alura.CoisasAFazer.Tests
{
    public class CadastraTarefaHandlerExecute
    {
        [Fact]
        public void InformadaTarefaValidaDeveIncluirnoDB()
        {
            //Arrange
            var command = new CadastraTarefa("Estudar Mock", new Categoria("Estudo"), new DateTime(2022, 03, 15));
            var mock = new Mock <ILogger<CadastraTarefaHandler>>();
            var options = new DbContextOptionsBuilder<DbTarefasContext>().UseInMemoryDatabase("DbTeste").Options;
            var context = new DbTarefasContext(options);
            var repo = new RepositorioTarefa(context);
            var handler = new CadastraTarefaHandler(repo, mock.Object); 

            //Act
            handler.Execute(command);
            
            //Assert
            var tarefa = repo.ObtemTarefas(t => t.Titulo == "Estudar Mock");
            Assert.NotNull(tarefa);
        }

        [Fact]
        public void QuandoExceptionLancadaIsSuccessFalse()
        {
            //arrange
            var mensagemDeErroEsperada = "Houve um erro na inclusao de tarefas";
            var excecaoEsperada = new Exception(mensagemDeErroEsperada);

            var comando = new CadastraTarefa("Estudar Xunit", new Categoria("Estudo"), new DateTime(2019, 12, 31));

            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            var mock = new Mock<IRepositorioTarefas>();

            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>()))
                .Throws(excecaoEsperada);

            var repo = mock.Object;

            var handler = new CadastraTarefaHandler(repo, mockLogger.Object);

            //act
            CommandResult resultado = handler.Execute(comando);

            //assert
            mockLogger.Verify(l =>
                    l.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.IsAny<object>(),
                        excecaoEsperada,
                        (Func<object, Exception, string>) It.IsAny<object>()),
                Times.Once());
        }
    }
}