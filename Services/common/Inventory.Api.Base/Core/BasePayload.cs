using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Api.Base.Core
{
    public interface IPayload
    {

        void AddError(object o);

        bool HasError();

    }

    public abstract class BasePayload<U, T> : IPayload where U : BasePayload<U, T>, new()
    {
        protected BasePayload()
        {
            this.errors = new List<T>();
        }

        /// <summary>
        /// List of possible union errors
        /// </summary>
        /// <value></value>
        public List<T> errors { get; set; }

        /// <summary>
        /// Add errors collection and return itself
        /// </summary>
        public U PushError(params T[] errors)
        {
            this.errors.AddRange(errors);

            return (U)this;
        }

        /// <summary>
        /// Check if any error exist
        /// </summary>
        public bool HasError()
        {

            if (errors != null)
            {
                return errors.Any();
            }

            return false;
        }

        /// <summary>
        /// Return new instance with errors
        /// </summary>
        /// <param name="errors"></param>
        public static U Error(params T[] errors)
        {
            U u = new U();
            u.errors.AddRange(errors);
            return u;
        }

        /// <summary>
        /// Returns new instance
        /// </summary>
        public static U Success()
        {
            return new U();
        }

        public void AddError(object o)
        {

            if (o is T)
            {
                T tmp = (T)o;
                this.errors.Add(tmp);
            }
            else
            {
                throw new NotSupportedException("Error type does not match base payload supported types");
            }
        }
    }
}
