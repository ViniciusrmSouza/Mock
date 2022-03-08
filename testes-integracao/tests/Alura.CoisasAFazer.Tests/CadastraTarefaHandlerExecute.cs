using System;
using System.Linq;
using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
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

            var options = new DbContextOptionsBuilder<DbTarefasContext>().UseInMemoryDatabase("DbTeste").Options;
            var context = new DbTarefasContext(options);
            var repo = new RepositorioTarefa(context);
            var handler = new CadastraTarefaHandler(repo); 

            //Act
            handler.Execute(command);
            
            //Assert
            var tarefa = repo.ObtemTarefas(t => t.Titulo == "Estudar Mock").FirstOrDefault();
            Assert.NotNull(tarefa);
        }
    }
}