using HotChocolate.Types;
using Inventory.Common.Application.Errors;
using Inventory.Common.Application.Exceptions;
using Inventory.Common.Application.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Graphql.Types
{
    public class ErrorTypes
    {
        //public class BaseErrorType : ObjectType<ApiError>
        //{
        //    protected override void Configure(IObjectTypeDescriptor<ApiError> descriptor)
        //    {

        //        descriptor.Implements<BaseErrorInterfaceType>();
        //    }
        //}

        public class BaseErrorInterfaceType : InterfaceType<IApiError>
        {
            protected override void Configure(IInterfaceTypeDescriptor<IApiError> descriptor)
            {
                descriptor.Name("ApiError");
                descriptor.Field(e => e.Message).Type<StringType>();
            }
        }

        //public class ValidationErrorType : ObjectType<ValidationError>
        //{
        //    protected override void Configure(IObjectTypeDescriptor<ValidationError> descriptor)
        //    {
        //        descriptor.Implements<BaseErrorInterfaceType>();
        //    }
        //}

        //public class UnAuthorisedErrorType : ObjectType<UnAuthorisedError>
        //{
        //    protected override void Configure(IObjectTypeDescriptor<UnAuthorisedError> descriptor)
        //    {
        //        descriptor.Implements<BaseErrorInterfaceType>();
        //    }
        //}

        //public class CreateWebHookErrorUnion : UnionType
        //{
        //    protected override void Configure(IUnionTypeDescriptor descriptor)
        //    {
        //        descriptor.Type<ValidationErrorType>();
        //        descriptor.Type<UnAuthorisedErrorType>();
        //        //descriptor.Type<InternalServerErrorType>();
        //    }
        //}

    }
}
