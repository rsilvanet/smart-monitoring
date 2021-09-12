using Moq;
using SmartMonitoring.Business.Exceptions;
using SmartMonitoring.Business.Repositories;
using SmartMonitoring.Business.Tests.Commands;
using SmartMonitoring.Business.UseCases.Create;
using SmartMonitoring.Domain;
using Xunit;

namespace SmartMonitoring.Business.Tests.UseCases
{
    public class CreateServiceUseCaseTests
    {
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly CreateServiceUseCase _createServiceUseCase;

        public CreateServiceUseCaseTests()
        {
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _createServiceUseCase = new CreateServiceUseCase(_serviceRepositoryMock.Object);
        }

        [Fact]
        public async void ShouldThrowIfNameIsAlreadyInUse()
        {
            var command = TestSaveServiceCommand.Create();

            _serviceRepositoryMock.Setup(x => x.ExistsAsync(command.Name)).ReturnsAsync(true);

            await Assert.ThrowsAsync<ServiceNameAlreadyInUseException>(() =>
            {
                return _createServiceUseCase.ExecuteAsync(command);
            });

            _serviceRepositoryMock.Verify(x => x.ExistsAsync(command.Name), Times.Once);
            _serviceRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Service>()), Times.Never);
        }

        [Fact]
        public async void ShouldCreateNewServiceAndAddToRepository()
        {
            var command = TestSaveServiceCommand.Create();
            var service = await _createServiceUseCase.ExecuteAsync(command);

            Assert.Equal(command.Name, service.Name);

            _serviceRepositoryMock.Verify(x => x.ExistsAsync(command.Name), Times.Once);
            _serviceRepositoryMock.Verify(x => x.AddAsync(service), Times.Once);
        }
    }
}
