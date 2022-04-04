using Inventory.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Models.Configuration
{
    public class ApplicationInstance : ValueObject
    {
        public string ApplicationCode { get; set; }
        public Environment Environment { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ApplicationCode;
            yield return Environment.Code;
        }
    }
}
