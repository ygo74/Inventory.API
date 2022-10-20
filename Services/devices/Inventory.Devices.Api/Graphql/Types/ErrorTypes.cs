using HotChocolate.Types;
using Inventory.Api.Base.Core;
using Inventory.Api.Base.Exceptions;
using Inventory.Api.Base.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Inventory.Devices.Api.Applications.Servers.CreateServer;

namespace Inventory.Devices.Api.Graphql.Types
{
    public class ErrorTypes
    {
        public class BaseErrorType : ObjectType<ApiError>
        {
            protected override void Configure(IObjectTypeDescriptor<ApiError> descriptor)
            {

                descriptor.Implements<BaseErrorInterfaceType>();
            }
        }

        public class BaseErrorInterfaceType : InterfaceType<IApiError>
        {
            protected override void Configure(IInterfaceTypeDescriptor<IApiError> descriptor)
            {
                descriptor.Field(e => e.message).Type<StringType>();
            }
        }

        public class ValidationErrorType : ObjectType<ValidationError>
        {
            protected override void Configure(IObjectTypeDescriptor<ValidationError> descriptor)
            {
                descriptor.Implements<BaseErrorInterfaceType>();
            }
        }

        public class UnAuthorisedErrorType : ObjectType<UnAuthorisedError>
        {
            protected override void Configure(IObjectTypeDescriptor<UnAuthorisedError> descriptor)
            {
                descriptor.Implements<BaseErrorInterfaceType>();
            }
        }

        //public class CreateWebHookErrorUnion : UnionType<IApiError>
        //{
        //    protected override void Configure(IUnionTypeDescriptor descriptor)
        //    {
        //        descriptor.Type<ValidationErrorType>();
        //        descriptor.Type<UnAuthorisedErrorType>();
        //    }
        //}
    }
}
