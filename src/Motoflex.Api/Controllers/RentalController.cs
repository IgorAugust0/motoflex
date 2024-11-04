using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Motoflex.Application.DTOs.Requests;
using Motoflex.Application.DTOs.Responses;
using Motoflex.Application.Interfaces;
using Motoflex.Application.Mappers;
using Motoflex.Domain.Entities;
using Motoflex.Domain.Interfaces.Notifications;

namespace Motoflex.Api.Controllers
{
    [ApiController]
    [Route("locacao")]
    [Produces("application/json")]
    [Tags("locação")]
    public class RentalController : AbstractController
    {
        private readonly IRentalService _service;
        private readonly INotificationContext _notificationContext;

        public RentalController(IRentalService service, INotificationContext notificationContext)
        {
            ArgumentNullException.ThrowIfNull(service, nameof(service));
            ArgumentNullException.ThrowIfNull(notificationContext, nameof(notificationContext));
            _service = service;
            _notificationContext = notificationContext;
        }

        /// <summary>
        /// Alugar uma moto
        /// </summary>
        /// <response code="201">Locação criada com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="403">Apenas entregadores podem alugar motos</response>
        [HttpPost]
        // [Authorize(Roles = Roles.Renter)]
        [ProducesResponseType(typeof(ResponseModel<Rental>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseModel<Rental?>>> Post([FromBody] CreateRental request)
        {
            var rental = request.Map();
            rental.RenterId = LoggedUserGuid(); // will only work after authentication is implemented

            var success = await _service.InsertRentalAsync(rental);

            if (!success || _notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Rental?>(null, _notificationContext.Notifications));

            // return CreatedAtAction(nameof(Post), new { id = rental.Id }, new ResponseModel<Rental>(rental));
            return CreatedAtAction(nameof(GetById), new { id = rental.Id }, new ResponseModel<Rental>(rental));
        }

        /// <summary>
        /// Consultar locação por id
        /// </summary>
        /// <response code="200">Retorna dados da locação</response>
        /// <response code="404">Locação não encontrada</response>
        /// <response code="403">Usuário não autorizado</response>
        [HttpGet("{id}")]
        // [Authorize(Roles = Roles.Renter)]
        [ProducesResponseType(typeof(ResponseModel<Rental>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseModel<Rental>>> GetById(Guid id)
        {
            var rental = await _service.GetByIdAsync(id);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Rental?>(null, _notificationContext.Notifications));

            if (rental == null) return NotFound();

            return Ok(new ResponseModel<Rental>(rental));
        }

        /// <summary>
        /// Informar data de devolução e calcular valor
        /// </summary>
        /// <response code="200">Retorna valor calculado</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="403">Apenas entregadores podem informar devolução</response>
        /// <response code="404">Locação não encontrada</response>
        [HttpPut("{id}/devolucao")]
        // [Authorize(Roles = Roles.Renter)]
        [ProducesResponseType(typeof(ResponseModel<decimal>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseModel<RentalPriceResponse>>> ReportReturn(
            Guid id,
            [FromBody] UpdateReturnDate request)
        {
            var price = await _service.ReportReturnAsync(
                id,
                request.ReturnDate,
                LoggedUserGuid()); // will only work after authentication is implemented

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<RentalPriceResponse?>(null, _notificationContext.Notifications));

            if (price == 0) return NotFound();

            return Ok(new ResponseModel<RentalPriceResponse>(new RentalPriceResponse(price)));
        }
    }
}
