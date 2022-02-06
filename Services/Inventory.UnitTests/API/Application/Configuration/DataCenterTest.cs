using MediatR;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.UnitTests.API.Application.Configuration
{
    [TestFixture]
    internal class DataCenterTest : BaseDbInventoryTests
    {

        private readonly IMediator _mediator;

        public DataCenterTest()
        {
            _mediator = this.GetMediator();
        }

    }
}
