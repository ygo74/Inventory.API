using Ardalis.Specification;
using Inventory.Domain.Models.Configuration;
using System.Linq;

namespace Inventory.Domain.Specifications
{
    public class ApplicationSpecification : Specification<Application>
    {
        public ApplicationSpecification()
        {
            Query.OrderBy(a => a.Name);
            Query.Include(a => a.Servers);
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    if (!string.IsNullOrWhiteSpace(_name))
                    {
                        Query.Where(a => a.Name == _name.ToLower());
                    }
                }
            }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code != value)
                {
                    _code = value;
                    if (!string.IsNullOrWhiteSpace(_code))
                    {
                        Query.Where(a => a.Code == _code.ToUpper());
                    }
                }
            }
        }

        private int[] _applicationIds { get; set; }
        public int[] ApplicationIds {
            get { return _applicationIds; }
            set
            {
                _applicationIds = value;
                Query.Where(a => _applicationIds.Contains(a.ApplicationId));
            }
        }


    }
}
