using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Motoflex.Application.DTOs.Responses;
using Motoflex.Application.Interfaces;
using Motoflex.Domain.Entities;

namespace Motoflex.Api.Controllers
{
    [ApiController]
    [Route("administradores")]
    [Produces("application/json")]
    [Tags("administradores")]
    [ApiExplorerSettings(IgnoreApi = true)] // comment this line to enable the controller
    public sealed class AdminController : AbstractController
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
        {
            ArgumentNullException.ThrowIfNull(service, nameof(service));
            _service = service;
        }

        /// <summary>
        /// Consultar administradores
        /// </summary>
        /// <response code="200">Lista de administradores</response>
        /// <response code="403">Usuário não autorizado</response>
        [HttpGet]
        // [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<Admin>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseModel<IEnumerable<Admin>>>> Get()
        {
            try
            {
                var admins = await _service.GetAsync();
                return Ok(new ResponseModel<IEnumerable<Admin>>(admins));
            }
            catch (Exception)
            {
                return BadRequest(new ResponseModel<Admin?>(null, ["Erro ao buscar administradores"]));
            }
        }

        /// <summary>
        /// Cadastrar administrador
        /// </summary>
        /// <response code="201">Administrador cadastrado com sucesso</response>
        /// <response code="400">Erro ao criar administrador</response>
        /// <response code="403">Usuário não autorizado</response>
        [HttpPost]
        // [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(ResponseModel<Admin>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ResponseModel<Admin>>> Post()
        {
            var admin = await _service.CreateAdminAsync();

            if (admin == null)
                return BadRequest(new ResponseModel<Admin?>(null, ["Erro ao criar administrador"]));

            return CreatedAtAction(nameof(Get), new { id = admin.Id },
                new ResponseModel<Admin>(admin));
        }
    }
}