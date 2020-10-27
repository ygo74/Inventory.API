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
            get { return _value; }
            set { _value = value; }

        }

        //public override object RawValue => _value;
    }
}
