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
    [Route("pedidos")]
    [Produces("application/json")]
    [Tags("pedidos")]
    [ApiExplorerSettings(IgnoreApi = true)] // comment this line to enable this controller
    public class OrderController : AbstractController
    {
        private readonly IOrderService _service;
        private readonly INotificationContext _notificationContext;

        public OrderController(IOrderService service, INotificationContext notificationContext)
        {
            ArgumentNullException.ThrowIfNull(service, nameof(service));
            ArgumentNullException.ThrowIfNull(notificationContext, nameof(notificationContext));
            _service = service;
            _notificationContext = notificationContext;
        }

        /// <summary>
        /// Consultar pedidos
        /// </summary>
        /// <response code="200">Lista de pedidos</response>
        /// <response code="403">Usuário não autorizado</response>
        [HttpGet]
        // [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<Order>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseModel<IEnumerable<Order>>>> Get()
        {
            var orders = await _service.GetAsync();
            return Ok(new ResponseModel<IEnumerable<Order>>(orders));
        }

        /// <summary>
        /// Consultar pedido por ID
        /// </summary>
        /// <response code="200">Pedido encontrado</response>
        /// <response code="404">Pedido não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseModel<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseModel<Order>>> GetById(Guid id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(new ResponseModel<Order>(order));
        }

        /// <summary>
        /// Consultar entregadores notificados por pedido
        /// </summary>
        /// <response code="200">Lista de entregadores notificados</response>
        /// <response code="404">Pedido não encontrado</response>
        /// <response code="403">Usuário não autorizado</response>
        [HttpGet("{id}/entregadores-notificados")]
        // [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<Renter>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseModel<IEnumerable<Renter>>>> GetNotifiedRenters(Guid id)
        {
            var order = await _service.GetNotifiedOrdersAsync(id);
            if (order == null) return NotFound();
            return Ok(new ResponseModel<IEnumerable<Renter>>(order.NotifiedRenters));
        }

        /// <summary>
        /// Cadastrar novo pedido
        /// </summary>
        /// <response code="201">Pedido cadastrado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="403">Usuário não autorizado</response>
        [HttpPost]
        // [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ResponseModel<Order>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseModel<Order?>>> Post([FromBody] CreateOrder request)
        {
            var order = request.Map();
            var result = await _service.InsertOrderAsync(order);

            if (result == null || _notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Order?>(null, _notificationContext.Notifications));

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, new ResponseModel<Order?>(result));
        }

        /// <summary>
        /// Aceitar pedido
        /// </summary>
        /// <response code="200">Pedido aceito com sucesso</response>
        /// <response code="400">Operação inválida</response>
        /// <response code="403">Usuário não autorizado</response>
        /// <response code="404">Pedido não encontrado</response>
        [HttpPut("{id}/aceitar")]
        // [Authorize(Roles = Roles.Renter)]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseModel<bool>>> Accept(Guid id)
        {
            var success = await _service.AcceptOrderAsync(id, LoggedUserGuid());

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<bool>(false, _notificationContext.Notifications));

            if (!success) return NotFound();

            return Ok(new ResponseModel<bool>(true));
        }

        /// <summary>
        /// Finalizar pedido
        /// </summary>
        /// <response code="200">Pedido finalizado com sucesso</response>
        /// <response code="400">Operação inválida</response>
        /// <response code="403">Usuário não autorizado</response>
        /// <response code="404">Pedido não encontrado</response>
        [HttpPut("{id}/finalizar")]
        // [Authorize(Roles = Roles.Renter)]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseModel<bool>>> Finish(Guid id)
        {
            var success = await _service.FinishOrderAsync(id, LoggedUserGuid());

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<bool>(false, _notificationContext.Notifications));

            if (!success) return NotFound();

            return Ok(new ResponseModel<bool>(true));
        }
    }
}
