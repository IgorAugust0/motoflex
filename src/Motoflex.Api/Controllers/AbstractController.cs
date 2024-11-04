using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Motoflex.Api.Controllers
{
    /// <summary>
    /// Abstract base controller providing common functionality for derived controllers.
    /// </summary>
    public abstract class AbstractController : ControllerBase
    {
        /// <summary>
        /// Retrieves the GUID of the logged-in user from the claims.
        /// </summary>
        /// <returns>The GUID of the logged-in user, or Guid.Empty if not found or invalid.</returns>
        protected Guid LoggedUserGuid()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userGuid) ? userGuid : Guid.Empty;
        }
    }
}
