using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartMonitoring.Business.Exceptions;
using SmartMonitoring.MemoryDatabase.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartMonitoring.MemoryDatabase.Tests
{
    public class ServiceRepositoryTests
    {
        private readonly SmartMonitoringDbContext _dbContext;
        private readonly SqlServiceRepository _sqlServiceRepository;

        public ServiceRepositoryTests()
        {
            var services = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var dbContextOptions = new DbContextOptionsBuilder<SmartMonitoringDbContext>()
                .UseInMemoryDatabase(databaseName: $"SmartMonitoring.{Guid.NewGuid()}")
                .UseInternalServiceProvider(services)
                .Options;

            _dbContext = new SmartMonitoringDbContext(dbContextOptions);
            _sqlServiceRepository = new SqlServiceRepository(_dbContext);

            GenerateInitialData();
        }

        private void GenerateInitialData()
        {
            _dbContext.Add(new Entities.Service()
            {
                Id = Guid.NewGuid(),
                Name = "service1",
                Port = 8080,
                Maintainer = "test@gmail.com",
                Labels = new List<Entities.Label>
                {
                    new Entities.Label("key1:value1"),
                    new Entities.Label("key2:value2")
                }
            });

            _dbContext.Add(new Entities.Service()
            {
                Id = Guid.NewGuid(),
                Name = "service2",
                Port = 8081,
                Maintainer = "test@gmail.com",
                Labels = new List<Entities.Label>
                {
                    new Entities.Label("key2:value2")
                }
            });

            _dbContext.Add(new Entities.Service()
            {
                Id = Guid.NewGuid(),
                Name = "service3",
                Port = 8082,
                Maintainer = "test@gmail.com",
                Labels = new List<Entities.Label>
                {
                    new Entities.Label("key3:value3")
                }
            });

            _dbContext.SaveChanges();
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnAllServices()
        {
            var services = await _sqlServiceRepository.GetAllAsync();

            Assert.Equal(3, services.Count());
            Assert.Contains(services, x => x.Name == "service1");
            Assert.Contains(services, x => x.Name == "service2");
            Assert.Contains(services, x => x.Name == "service3");
        }

        [Fact]
        public async void GetByLabelAsync_ShouldReturnOnlyServicesWithTheSpecifiedLabel()
        {
            var services = await _sqlServiceRepository.GetByLabelAsync("key2:value2");

            Assert.Equal(2, services.Count());
            Assert.Contains(services, x => x.Name == "service1");
            Assert.Contains(services, x => x.Name == "service2");
        }

        [Fact]
        public async void ExistsAsync_ShouldReturnTrueIfServiceExists()
        {
            Assert.True(await _sqlServiceRepository.ExistsAsync("service1"));
        }

        [Fact]
        public async void ExistsAsync_ShouldReturnFalseIfServiceDoesNotExist()
        {
            Assert.False(await _sqlServiceRepository.ExistsAsync("service99"));
        }

        [Fact]
        public async void GetByNameAsync_ShouldReturnTheCorrectService()
        {
            var service = await _sqlServiceRepository.GetByNameAsync("service3");

            Assert.Equal("service3", service.Name);
        }

        [Fact]
        public async void GetByNameAsync_ShouldThrowIfServiceDoesNotExist()
        {
            await Assert.ThrowsAsync<ServiceNotFoundException>(() =>
            {
                return _sqlServiceRepository.GetByNameAsync("service99");
            });
        }

        [Fact]
        public async void AddAsync_ShouldAddToDatabase()
        {
            var service = new Domain.Service(
                name: "service4",
                port: 8083,
                maintainer: "test@gmail.com",
                labels: new List<Domain.ValueObjects.Label> { "key1:value1" }
            );

            Assert.False(_dbContext.Services.Any(x => x.Name == "service4"));

            await _sqlServiceRepository.AddAsync(service);

            Assert.True(_dbContext.Services.Any(x => x.Name == "service4"));
        }

        [Fact]
        public async void UpdateAsync_ShouldUpdateInDatabase()
        {
            var service = await _sqlServiceRepository.GetByNameAsync("service1");

            service.Update(
                name: "service99",
                port: 8099,
                maintainer: "test99@gmail.com",
                labels: new List<Domain.ValueObjects.Label> { "key99:value99" }
            );

            await _sqlServiceRepository.UpdateAsync(service);

            var serviceAfterUpdate = await _dbContext.Services.FindAsync(service.Id);

            Assert.Equal("service99", serviceAfterUpdate.Name);
            Assert.Equal(8099, serviceAfterUpdate.Port);
            Assert.Equal("test99@gmail.com", serviceAfterUpdate.Maintainer);
            Assert.Single(serviceAfterUpdate.Labels);
            Assert.Equal("key99:value99", serviceAfterUpdate.Labels.ElementAt(0).Value);
        }

        [Fact]
        public async void DeleteAsync_ShouldDeleteFromDatabase()
        {
            Assert.True(_dbContext.Services.Any(x => x.Name == "service1"));

            await _sqlServiceRepository.DeleteAsync("service1");

            Assert.False(_dbContext.Services.Any(x => x.Name == "service1"));
        }
    }
}
