using Moq;
using SmartMonitoring.Business.Exceptions;
using SmartMonitoring.Business.Repositories;
using SmartMonitoring.Business.UseCases.Create;
using SmartMonitoring.Domain.ValueObjects;
using Xunit;

namespace SmartMonitoring.Business.Tests.UseCases
{
    public class DeleteServiceUseCaseTests
    {
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly DeleteServiceUseCase _deleteServiceUseCase;

        public DeleteServiceUseCaseTests()
        {
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _deleteServiceUseCase = new DeleteServiceUseCase(_serviceRepositoryMock.Object);
        }

        [Fact]
        public async void ShouldThrowIfServiceIsNotFound()
        {
            var serviceName = new Name("service1");

            _serviceRepositoryMock.Setup(x => x.ExistsAsync(serviceName)).ReturnsAsync(false);

            await Assert.ThrowsAsync<ServiceNotFoundException>(() =>
            {
                return _deleteServiceUseCase.ExecuteAsync(serviceName);
            });

            _serviceRepositoryMock.Verify(x => x.ExistsAsync(serviceName), Times.Once);
            _serviceRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Name>()), Times.Never);
        }

        [Fact]
        public async void ShouldCallRepositoryToDeleteService()
        {
            var serviceName = new Name("service1");

            _serviceRepositoryMock.Setup(x => x.ExistsAsync(serviceName)).ReturnsAsync(true);

            await _deleteServiceUseCase.ExecuteAsync(serviceName);

            _serviceRepositoryMock.Verify(x => x.ExistsAsync(serviceName), Times.Once);
            _serviceRepositoryMock.Verify(x => x.DeleteAsync(serviceName), Times.Once);
        }
    }
}
