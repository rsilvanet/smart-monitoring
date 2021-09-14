using Microsoft.AspNetCore.Mvc;
using SmartMonitoring.API.Models.Requests;
using SmartMonitoring.API.Models.Responses;
using SmartMonitoring.Business.Repositories;
using SmartMonitoring.Business.UseCases.Create;
using SmartMonitoring.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMonitoring.API.Controllers
{
    [ApiController]
    [Route("services")]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly CreateServiceUseCase _createUseCase;
        private readonly UpdateServiceUseCase _updateUseCase;
        private readonly DeleteServiceUseCase _deleteUseCase;

        public ServicesController(
            IServiceRepository serviceRepository,
            CreateServiceUseCase createUseCase,
            UpdateServiceUseCase updateUseCase,
            DeleteServiceUseCase deleteUseCase
            )
        {
            _serviceRepository = serviceRepository;
            _createUseCase = createUseCase;
            _updateUseCase = updateUseCase;
            _deleteUseCase = deleteUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ServiceResponse[]), 200)]
        public async Task<IActionResult> Get(string labels)
        {
            IEnumerable<Service> services;

            if (string.IsNullOrWhiteSpace(labels))
            {
                services = await _serviceRepository.GetAllAsync();
            }
            else
            {
                services = await _serviceRepository.GetByLabelAsync(labels);
            }

            return Ok(services.Select(s => new ServiceResponse(s)));
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(ServiceResponse), 200)]
        public async Task<IActionResult> GetByName(string name)
        {
            var service = await _serviceRepository.GetByNameAsync(name);
            return Ok(new ServiceResponse(service));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServiceResponse), 201)]
        public async Task<IActionResult> Create([FromBody] ServiceRequest request)
        {
            var service = await _createUseCase.ExecuteAsync(request);
            return StatusCode(201, new ServiceResponse(service));
        }

        [HttpPut("{name}")]
        [ProducesResponseType(typeof(ServiceResponse), 200)]
        public async Task<IActionResult> Update(string name, [FromBody] ServiceRequest request)
        {
            var service = await _updateUseCase.ExecuteAsync(name, request);
            return Ok(new ServiceResponse(service));
        }

        [HttpDelete("{name}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete(string name)
        {
            await _deleteUseCase.ExecuteAsync(name);
            return Ok();
        }
    }
}
