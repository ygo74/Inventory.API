using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Models
{
    public class NumericVariable : Variable
    {

        private Int64 _value;

        public Int64 Value
        {
            get;set;

        }
        
    }
}
