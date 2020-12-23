using Inventory.Domain.Models;
using NUnit.Framework;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.UnitTests.Domain
{
    public class ServerTest
    {
        [Test]
        public void Allow_Add_New_Windows_disk()
        {
            //Get server
            var os = new Inventory.Domain.Models.OperatingSystem("Windows 2019", OsFamilly.Windows);
            var env = new Inventory.Domain.Models.Environment("POC", EnvironmentFamilly.Developoment);
            var server = new Server("test1", os, env, 2, 4, System.Net.IPAddress.Parse("192.168.1.0"));

            //Test add New DIsk
            server.AddOrUpdateWindowsDisk("Data", 100, 'D', "");

            var newDisk = (WindowsDisk)server.ServerDisks.FirstOrDefault(d => d.Name == "Data");
            Assert.IsNotNull(newDisk);
            Assert.AreEqual(100, newDisk.Size);
            Assert.AreEqual('D', newDisk.Letter);
            Assert.AreEqual("Data", newDisk.Label);
        }

        [Test]
        public void Allow_Update_existing_Windows_disk()
        {
            //Get server
            var os = new Inventory.Domain.Models.OperatingSystem("Windows 2019", OsFamilly.Windows);
            var env = new Inventory.Domain.Models.Environment("POC", EnvironmentFamilly.Developoment);
            var server = new Server("test1", os, env, 2, 4, System.Net.IPAddress.Parse("192.168.1.0"));

            //Test add New DIsk
            server.AddOrUpdateWindowsDisk("Data", 150, 'C', "");

            Assert.IsTrue(server.ServerDisks.Count == 1);
            var existingDisk = (WindowsDisk)server.ServerDisks.FirstOrDefault(d => ((WindowsDisk)d).Letter == 'C');
            Assert.IsNotNull(existingDisk);
            Assert.AreEqual(150, existingDisk.Size);
            Assert.AreEqual('C', existingDisk.Letter);
            Assert.AreEqual("system", existingDisk.Name);
        }



    }
}
