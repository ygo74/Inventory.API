using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Infrastructure
{
    public class GraphQLService
    {

        private readonly IAsyncRepository<Domain.Models.OperatingSystem> osRepository;

        public GraphQLService(IAsyncRepository<Domain.Models.OperatingSystem> _osRepository)
        {
            _osRepository = osRepository ?? throw new ArgumentNullException(nameof(osRepository));
        }


        #region "Operating System"



        #endregion

    }
}
