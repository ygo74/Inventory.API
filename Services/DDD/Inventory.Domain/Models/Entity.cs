using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Models
{
    public abstract class Entity
    {
        private Int64 _id;

        public virtual Int64 Id
        {
            get
            {
                return _id;
            }
            protected set
            {
                _id = value;
            }
        }


    }
}
