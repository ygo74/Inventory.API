using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Models
{
    public class WindowsDisk : BaseDisk
    {
        public Char Letter { get; private set; }
        public String Label { get; private set; }


        public WindowsDisk(string name, int size, Char letter, string label=""):base(name, size, "NTFS")
        {
            Letter = letter;
            Label = !String.IsNullOrEmpty(label) ? label : name;
        }

        public void SetLabel(string label)
        {
            Label = !String.IsNullOrEmpty(label) ? label : throw new ArgumentNullException(nameof(label));
        }


    }
}
