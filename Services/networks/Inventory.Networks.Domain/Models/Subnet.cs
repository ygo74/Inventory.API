﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Networks.Domain.Models
{
    public class Subnet
    {
        public string CIDR { get; private set; }

        public string ProviderId { get; private set; }

        public string Provider { get; private set; }


    }
}
