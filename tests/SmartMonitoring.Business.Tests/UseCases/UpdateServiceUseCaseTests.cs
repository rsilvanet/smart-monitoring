using Moq;
using SmartMonitoring.Business.Exceptions;
using SmartMonitoring.Business.Repositories;
using SmartMonitoring.Business.Tests.Commands;
using SmartMonitoring.Business.UseCases.Create;
using SmartMonitoring.Domain;
using SmartMonitoring.Domain.ValueObjects;
using System.Collections.Generic;
using Xunit;

namespace SmartMonitoring.Business.Tests.UseCases
{
    public class UpdateServiceUseCaseTests
    {
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly UpdateServiceUseCase _updateServiceUseCase;

        public UpdateServiceUseCaseTests()
        {
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _updateServiceUseCase = new UpdateServiceUseCase(_serviceRepositoryMock.Object);
        }

        private Service GetExistentService(string name = "service1", int port = 8080)
        {
            var labels = new List<Label>
            {
                "key1:value1"
            };

            return new Service(name, port, "test@gmail.com", labels);
        }

        [Fact]
        public async void ShouldThrowIfServiceIsNotFound()
        {
            var command = TestSaveServiceCommand.Create();
            var nonExistentServiceName = "nonExistentService";

            _serviceRepositoryMock.Setup(x => x.GetByNameAsync(nonExistentServiceName)).ReturnsAsync(null as Service);

            await Assert.ThrowsAsync<ServiceNotFoundException>(() =>
            {
                return _updateServiceUseCase.ExecuteAsync(nonExistentServiceName, command);
            });

            _serviceRepositoryMock.Verify(x => x.GetByNameAsync(nonExistentServiceName), Times.Once);
            _serviceRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Service>()), Times.Never);
        }

        [Fact]
        public async void ShouldThrowIfNameIsAlreadyInUse()
        {
            var currentService = GetExistentService(name: "currentName");
            var command = TestSaveServiceCommand.Create(name: "nameAlreadyInUseByOtherService");

            _serviceRepositoryMock.Setup(x => x.GetByNameAsync(currentService.Name)).ReturnsAsync(currentService);
            _serviceRepositoryMock.Setup(x => x.ExistsAsync(command.Name)).ReturnsAsync(true);

            await Assert.ThrowsAsync<ServiceNameAlreadyInUseException>(() =>
            {
                return _updateServiceUseCase.ExecuteAsync(currentService.Name, command);
            });

            _serviceRepositoryMock.Verify(x => x.GetByNameAsync(currentService.Name), Times.Once);
            _serviceRepositoryMock.Verify(x => x.ExistsAsync(command.Name), Times.Once);
            _serviceRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Service>()), Times.Never);
        }

        [Fact]
        public async void ShouldUpdateServiceAndCallRepository()
        {
            var currentService = GetExistentService(name: "service1", port: 8080);
            var command = TestSaveServiceCommand.Create(name: "service2", port: 8081);

            _serviceRepositoryMock.Setup(x => x.GetByNameAsync(currentService.Name)).ReturnsAsync(currentService);

            var returnedService = await _updateServiceUseCase.ExecuteAsync(currentService.Name, command);

            Assert.Equal(command.Name, returnedService.Name);
            Assert.Equal(command.Port, returnedService.Port);

            _serviceRepositoryMock.Verify(x => x.GetByNameAsync("service1"), Times.Once);
            _serviceRepositoryMock.Verify(x => x.ExistsAsync(command.Name), Times.Once);
            _serviceRepositoryMock.Verify(x => x.UpdateAsync(returnedService), Times.Once);
        }
    }
}
