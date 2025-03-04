using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using OrderService.Config;
using OrderService.Data;
using OrderService.Models;
using OrderService.Repositories;
using OrderService.Services;

namespace OrderService.Tests
{
    public class TestBase : IDisposable
    {
        protected IServiceProvider ServiceProvider { get; private set; }
        private string _databaseName = Guid.NewGuid().ToString(); // Nombre único por instancia

        public TestBase()
        {
            var serviceCollection = new ServiceCollection();

            // Configurar DbContext con InMemoryDatabase único por instancia
            serviceCollection.AddDbContext<ErpDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));

            // Registrar servicios necesarios
            serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
            serviceCollection.AddScoped<IOrderService, OrderManagementService>();
            serviceCollection.AddAutoMapper(typeof(Program)); // Ajusta según tu configuración

            // Registrar Utilities con Moq
            var mockUtilities = new Mock<Utilities>();
            mockUtilities.Setup(u => u.GenerateJwtToken(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                        .Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."); // Token ficticio para pruebas
            serviceCollection.AddScoped(_ => mockUtilities.Object);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public void Dispose()
        {
            // Limpiar la base de datos in-memory al finalizar
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ErpDbContext>();
                context.Database.EnsureDeleted();
            }
        }
    }
}