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
            get;set;
        }
        
    }
}
