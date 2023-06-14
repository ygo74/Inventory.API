using Inventory.Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Infrastructure.Database
{
    public class NoCurrentUser : ICurrentUser
    {
        public bool Exist => false;

        public Guid? UserId => Guid.Empty;

        public string Name => string.Empty;

        public string JwtToken => string.Empty;

        public ClaimsIdentity Claims => null;

        public bool IsAuthenticated => false;

        public string GetClaim(string type)
        {
            return string.Empty;
        }

        public bool HasRole(string role_name)
        {
            return false;
        }
    }
}
