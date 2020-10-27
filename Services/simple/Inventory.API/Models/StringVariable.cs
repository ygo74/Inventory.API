using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Models
{
    public class StringVariable : Variable
    {

        private String _value;

        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }

        //public override object RawValue => _value;
    }
}
