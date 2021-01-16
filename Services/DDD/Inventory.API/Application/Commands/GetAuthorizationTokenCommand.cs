using Inventory.API.Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Application.Commands
{
    public class GetAuthorizationTokenCommand : IRequest<String>
    {

    }
}
