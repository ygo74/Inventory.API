using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Models
{
    public class BaseDisk
    {
        public int BaseDiskId { get; private set; }

        public int Size { get; private set; }
        public string Name { get; private set; }
        public string Format { get; private set; }

        public BaseDisk(string name, int size, string format)
        {
            Name = !String.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
            Size = size > 0 ? size : throw new ArgumentNullException(nameof(size));
            Format = !String.IsNullOrWhiteSpace(format) ? name : throw new ArgumentNullException(nameof(format));
        }

        public void SetSize(int size)
        {
            Size = size > 0 ? size : throw new ArgumentNullException(nameof(size));            
        }


    }
}
