using Microsoft.AspNetCore.Mvc;
using Motoflex.Application.DTOs.Responses;
using Motoflex.Application.Interfaces;
using Motoflex.Domain.Constants;
using Motoflex.Domain.Interfaces.Notifications;
using Motoflex.Domain.Utilities;

namespace Motoflex.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)] // comment this line to enable the controller
    public class AuthController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IRenterService _renterService;
        private readonly ITokenService _tokenService;
        private readonly INotificationContext _notificationContext;

        public AuthController(
            IAdminService adminService,
            IRenterService renterService,
            ITokenService tokenService,
            INotificationContext notificationContext)
        {
            ArgumentNullException.ThrowIfNull(adminService, nameof(adminService));
            ArgumentNullException.ThrowIfNull(renterService, nameof(renterService));
            ArgumentNullException.ThrowIfNull(tokenService, nameof(tokenService));
            ArgumentNullException.ThrowIfNull(notificationContext, nameof(notificationContext));

            _adminService = adminService;
            _renterService = renterService;
            _tokenService = tokenService;
            _notificationContext = notificationContext;
        }

        public async Task<ActionResult<ResponseModel<AuthenticationResponse>>> Login(Guid id)
        {
            if (id == Guid.Empty)
            {
                _notificationContext.AddNotification(ErrorNotifications.InvalidId);
                return BadRequest(new ResponseModel<object?>(null, _notificationContext.Notifications));
            }

            try
            {
                // Check if user is admin
                var admins = await _adminService.GetAsync();
                var admin = admins.FirstOrDefault(a => a.Id == id);
                if (admin != null)
                {
                    var adminToken = _tokenService.GenerateToken(admin.Id, Roles.Admin);
                    return Ok(new ResponseModel<AuthenticationResponse>(new AuthenticationResponse(adminToken)));
                }

                // Check if user is renter
                var renter = await _renterService.GetByIdAsync(id);
                if (renter != null)
                {
                    var renterToken = _tokenService.GenerateToken(id, Roles.Renter);
                    return Ok(new ResponseModel<AuthenticationResponse>(new AuthenticationResponse(renterToken)));
                }

                return NotFound();
            }
            catch (Exception)
            {
                _notificationContext.AddNotification(ErrorNotifications.AuthenticationError);
                return BadRequest(new ResponseModel<object?>(null, _notificationContext.Notifications));
            }
        }
    }
}
