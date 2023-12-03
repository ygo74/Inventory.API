using HotChocolate;
using Inventory.Common.Application.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Core
{
    public interface IPayload
    {

        void AddError(IApiError error);

        bool HasError();

    }


    public interface IPayload<T> : IPayload where T : class
    {

        void AddError(IApiError error);

        bool HasError();

        public T Data { get; set; }

    }

    public class Payload<T> : IPayload<T> where T : class
    {
        public Payload()
        {
            this.Errors = new List<IApiError>();
        }

        public T Data { get; set; }

        public List<IApiError> Errors { get; set; }

        [GraphQLIgnore()]
        public bool HasError()
        {
            return this.Errors.Any();
        }

        /// <summary>
        /// Add errors collection and return itself
        /// </summary>
        [GraphQLIgnore()]
        public Payload<T> PushError(params IApiError[] errors)
        {
            this.Errors.AddRange(errors);

            return this;
        }

        /// <summary>
        /// Return new instance with errors
        /// </summary>
        /// <param name="errors"></param>
        [GraphQLIgnore()]
        public static Payload<T> Error(params IApiError[] errors)
        {
            var entity = new Payload<T>();
            entity.Errors.AddRange(errors);
            return entity;
        }

        /// <summary>
        /// Returns new instance
        /// </summary>
        [GraphQLIgnore()]
        public static Payload<T> Success(T data)
        {
            var entity = new Payload<T>();
            entity.Data = data;
            return entity;
        }

        [GraphQLIgnore()]
        public void AddError(IApiError error)
        {
            this.Errors.Add(error);
            
        }


    }

}
