using Inventory.Common.Domain.Interfaces;
using System;
using System.Security.Claims;

namespace Inventory.Common.UnitTests
{
    public class CurrentTestUserService : ICurrentUser
    {

        private readonly Guid _userGuid = Guid.Empty;
        private readonly string _userToken = "XXXXX";


        public bool Exist => true;

        public Guid? UserId => _userGuid;

        public string Name => "Test User";

        public string JwtToken => _userToken;

        public ClaimsIdentity Claims => new ClaimsIdentity();

        public bool IsAuthenticated => true;

        public string GetClaim(string type)
        {
            throw new NotImplementedException();
        }

        public bool HasRole(string role_name)
        {
            return true;
        }
    }
}
