﻿using Ardalis.Specification;
using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Specifications
{
    public class GroupSpecification : Specification<Group>
    {
        public GroupSpecification()
        {
            Query.Include(g => g.Parent);
        }


        public GroupSpecification(string name)
        {
            Query
                .Where(g => g.Name == name.ToLower());
        }
    }
}
