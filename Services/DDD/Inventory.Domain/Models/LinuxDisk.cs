using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Models
{
    public class LinuxDisk : BaseDisk
    {
        public String Path { get; private set; }
        public String Datavg { get; private set; }

        public LinuxDisk(string name, int size, String datavg, string path) : base(name, size, "NFS")
        {
            Path   = !String.IsNullOrEmpty(path)   ? path   : throw new ArgumentNullException(nameof(path));
            Datavg = !String.IsNullOrEmpty(datavg) ? datavg : throw new ArgumentNullException(nameof(datavg));
        }

        public void SetPath(string path)
        {
            Path = !String.IsNullOrEmpty(path) ? path : throw new ArgumentNullException(nameof(path));
        }

        public void SetDatavg(string datavg)
        {
            Datavg = !String.IsNullOrEmpty(datavg) ? datavg : throw new ArgumentNullException(nameof(datavg));
        }

    }
}
