namespace DigitalRepository.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    /// <summary>
    /// Defines the <see cref="CommonController" />
    /// </summary>
    public class CommonController : ControllerBase
    {
        /// <summary>
        /// The GetUserId
        /// </summary>
        /// <returns>The <see cref="long"/></returns>
        protected long GetUserId()
        {
            Claim? claimId = User.FindFirst(ClaimTypes.NameIdentifier);

            return claimId != null ? long.Parse(claimId.Value) : 0;
        }
    }
}
