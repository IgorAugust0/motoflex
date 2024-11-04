using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Motoflex.Application.DTOs.Requests;
using Motoflex.Application.DTOs.Responses;
using Motoflex.Application.Interfaces;
using Motoflex.Application.Mappers;
using Motoflex.Domain.Entities;
using Motoflex.Domain.Interfaces.Notifications;
using Motoflex.Domain.Utilities;
using File = Motoflex.Domain.Utilities.File;

namespace Motoflex.Api.Controllers
{
    [ApiController]
    [Route("entregadores")]
    [Produces("application/json")]
    [Tags("entregadores")]
    public class RenterController : ControllerBase
    {
        private readonly IRenterService _service;
        private readonly INotificationContext _notificationContext;

        public RenterController(IRenterService service, INotificationContext notificationContext)
        {
            ArgumentNullException.ThrowIfNull(service, nameof(service));
            ArgumentNullException.ThrowIfNull(notificationContext, nameof(notificationContext));
            _service = service;
            _notificationContext = notificationContext;
        }

        /// <summary>
        /// Cadastrar entregador
        /// </summary>
        /// <response code="201">Entregador cadastrado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseModel<Renter>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel<Renter?>>> Post([FromBody] CreateRenter request)
        {
            var renter = request.Map();
            var success = await _service.InsertRenterAsync(renter);

            if (!success || _notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Renter?>(null, _notificationContext.Notifications));

            // return CreatedAtAction(nameof(Post), new ResponseModel<Renter>(renter));
            return CreatedAtAction(nameof(Post), new { id = renter.Id }, new ResponseModel<Renter>(renter));
        }

        /// <summary>
        /// Enviar foto da CNH
        /// </summary>
        /// <response code="200">Foto da CNH atualizada com sucesso</response>
        /// <response code="400">Arquivo inválido</response>
        /// <response code="404">Entregador não encontrado</response>
        [HttpPost("{id}/cnh")]
        [ProducesResponseType(typeof(ResponseModel<Renter>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [Authorize(Roles = Roles.Renter)]
        public async Task<ActionResult<ResponseModel<Renter>>> UploadCnh(Guid id, IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                _notificationContext.AddNotification(ErrorNotifications.ImageRequired);
                return BadRequest(new ResponseModel<Renter?>(null, _notificationContext.Notifications));
            }

            var file = new File(
                image.OpenReadStream(),
                Path.GetFileNameWithoutExtension(image.FileName),
                Path.GetExtension(image.FileName).TrimStart('.')
            );

            // TODO: replace id with LoggedUserGuid() from AbstractController after implementing authentication
            var renter = await _service.UpdateRenterCnhImageAsync(id, file);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Renter?>(null, _notificationContext.Notifications));

            if (renter == null) return NotFound();

            return Ok(new ResponseModel<Renter>(renter));
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<ResponseModel<IEnumerable<Renter>>>> Get()
        {
            var renters = await _service.GetAsync();
            return Ok(new ResponseModel<IEnumerable<Renter>>(renters));
        }
    }
}
