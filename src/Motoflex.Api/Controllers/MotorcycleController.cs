using Microsoft.AspNetCore.Mvc;
using Motoflex.Application.DTOs.Requests;
using Motoflex.Application.DTOs.Responses;
using Motoflex.Application.Interfaces;
using Motoflex.Domain.Entities;
using Motoflex.Domain.Interfaces.Notifications;
using Motoflex.Application.Mappers;
using TGolla.Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;

namespace Motoflex.Api.Controllers
{
    [ApiController]
    //[Authorize(Roles = Roles.Admin)]
    // [Route("api/[controller]")]
    [Route("motos")]
    [Produces("application/json")]
    [Tags("motos")]
    // [SwaggerControllerOrder(1)]
    public sealed class MotorcycleController : ControllerBase
    {
        private readonly IMotorcycleService _service;
        private readonly INotificationContext _notificationContext;

        public MotorcycleController(IMotorcycleService service, INotificationContext notificationContext)
        {
            ArgumentNullException.ThrowIfNull(service, nameof(service));
            ArgumentNullException.ThrowIfNull(notificationContext, nameof(notificationContext));
            _service = service; // ?? throw new ArgumentNullException(nameof(service));
            _notificationContext = notificationContext;
        }

        /// <summary>
        /// Cadastrar uma nova moto
        /// </summary>
        /// <response code="201">Moto cadastrada com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="403">Usuário não autorizado</response>
        [HttpPost]
        // [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ResponseModel<Motorcycle>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseModel<Motorcycle?>>> Post([FromBody] CreateMotorcycle request)
        {
            var motorcycle = request.Map();
            var success = await _service.InsertMotorcycleAsync(motorcycle);

            if (!success || _notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Motorcycle?>(null, _notificationContext.Notifications)); // [.. _notificationContext.Notifications]

            // return CreatedAtAction(nameof(Post), new ResponseModel<Motorcycle>(motorcycle));
            return CreatedAtAction(nameof(GetById), new { id = motorcycle.Id }, new ResponseModel<Motorcycle>(motorcycle));
        }

        /// <summary>
        /// Consultar motos existentes
        /// </summary>
        /// <response code="200">Lista de motos</response>
        /// <response code="403">Usuário não autorizado</response>
        [HttpGet]
        // [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<Motorcycle>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseModel<IEnumerable<Motorcycle>>>> Get([FromQuery] string? licensePlate)
        {
            var motorcycles = await _service.GetByLicensePlateAsync(licensePlate ?? string.Empty);

            // return empty list if licensePlate is empty
            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<IEnumerable<Motorcycle>>([], _notificationContext.Notifications));

            return Ok(new ResponseModel<IEnumerable<Motorcycle>>(motorcycles));
        }

        /// <summary>
        /// Modificar a placa de uma moto
        /// </summary>
        /// <response code="200">Placa atualizada com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="404">Moto não encontrada</response>
        /// <response code="403">Usuário não autorizado</response>
        [HttpPut("{id}/placa")]
        // [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ResponseModel<Motorcycle>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseModel<Motorcycle>>> UpdateMotorcycle(Guid id, [FromBody] UpdateMotorcycle request)
        {
            var motorcycle = await _service.UpdateMotorcycleLicensePlateAsync(id, request.LicensePlate!);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Motorcycle?>(null, _notificationContext.Notifications));

            if (motorcycle == null) return NotFound();

            return Ok(new ResponseModel<Motorcycle?>(motorcycle));
        }

        /// <summary>
        /// Consultar motos existentes por id
        /// </summary>
        /// <response code="200">Moto encontrada</response>
        /// <response code="404">Moto não encontrada</response>
        /// <response code="403">Usuário não autorizado</response>
        [HttpGet("{id}")]
        // [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ResponseModel<Motorcycle>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseModel<Motorcycle>>> GetById(Guid id)
        {
            try
            {
                var motorcycle = await _service.GetMotorcycleByIdAsync(id);
                return Ok(new ResponseModel<Motorcycle>(motorcycle));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Remover uma moto
        /// </summary>
        /// <response code="204">Moto removida com sucesso</response>
        /// <response code="400">Não é possível remover a moto</response>
        /// <response code="404">Moto não encontrada</response>
        /// <response code="403">Usuário não autorizado</response>
        [HttpDelete("{id}")]
        // [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteMotorcycleAsync(id);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<object?>(null, _notificationContext.Notifications));

            if (!success) return NotFound();

            return NoContent();
        }
    }
}
