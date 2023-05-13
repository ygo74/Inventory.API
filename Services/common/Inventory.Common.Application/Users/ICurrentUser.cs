using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Users
{
    /// <summary>
    /// Current user provider
    /// </summary>
    public interface ICurrentUser
    {

        /// <summary>
        /// Returns true if user exist in context
        /// </summary>
        bool Exist { get; }
#nullable enable
        /// <summary>
        /// Returns curren user system Id
        /// </summary>
        Guid? UserId { get; }
#nullable disable
        /// <summary>
        /// Returns user name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns current api access token or null
        /// </summary>
        string JwtToken { get; }

#nullable enable
        /// <summary>
        /// Returns user claims
        /// </summary>
        ClaimsIdentity? Claims { get; }
#nullable disable

        /// <summary>
        /// Get specific claim value
        /// </summary>
        string GetClaim(string type);

        /// <summary>
        /// Test user regarding to specific role
        /// </summary>
        /// <param name="role_name"></param>
        /// <returns></returns>
        bool HasRole(string role_name);

        /// <summary>
        /// Returns API authentication state
        /// </summary>
        public bool IsAuthenticated { get; }

    }
}
