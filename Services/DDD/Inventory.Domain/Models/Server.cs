using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Inventory.Domain.Models
{
    public class Server : Entity
    {

        private const string WINDOWS_DISK_SYSTEM_NAME = "system";
        private const char WINDOWS_DISK_SYSTEM_LETTER = 'C';
        private const int WINDOWS_DISK_SYSTEM_SIZE = 100;

        private const string LINUX_DISK_SYSTEM_NAME = "root";
        private const string LINUX_DISK_SYSTEM_DATAVG = "datavg-root";
        private const string LINUX_DISK_SYSTEM_PATH = "/";
        private const int LINUX_DISK_SYSTEM_SIZE = 100;

        protected Server()
        {
            _serverGroups = new List<ServerGroup>();
        }

        public Server(String hostName, OperatingSystem operatingSystem, Environment environment, int cpu, int ram, System.Net.IPAddress subnet) : this()
        {
            HostName = !String.IsNullOrEmpty(hostName) ? hostName.ToLower() : throw new ArgumentNullException(nameof(hostName));
            OperatingSystem = operatingSystem ?? throw new ArgumentNullException(nameof(operatingSystem));
            Subnet = subnet ?? throw new ArgumentNullException(nameof(subnet));
            CPU = cpu > 0 ? cpu : throw new ArgumentNullException(nameof(cpu));
            RAM = ram > 0 ? ram : throw new ArgumentNullException(nameof(ram));

            // Add system disk to new server
            if (operatingSystem.Familly == OsFamilly.Windows)
            {
                AddOrUpdateWindowsDisk(WINDOWS_DISK_SYSTEM_NAME, WINDOWS_DISK_SYSTEM_SIZE, WINDOWS_DISK_SYSTEM_LETTER);
            }
            else
            {
                AddOrUpdateLinuxDisk(LINUX_DISK_SYSTEM_NAME, LINUX_DISK_SYSTEM_SIZE, LINUX_DISK_SYSTEM_DATAVG, LINUX_DISK_SYSTEM_PATH);
            }

            // Link server to Environment
            LinkToEnvironment(environment);

        }

        public int ServerId { get; private set; }
        public string HostName { get; private set; }

        public int CPU { get; private set; }
        public int RAM { get; private set; }
        public System.Net.IPAddress Subnet { get; private set; }

        //OS Familly
        public int OperatingSystemId { get; private set; }
        public OperatingSystem OperatingSystem { get; private set; }

        // Server Groups Link
        private List<ServerGroup> _serverGroups;
        public ICollection<ServerGroup> ServerGroups => _serverGroups.AsReadOnly();

        // Many to Many Environments
        private List<ServerEnvironment> _serverEnvironments = new List<ServerEnvironment>();
        public ICollection<ServerEnvironment> ServerEnvironments => _serverEnvironments.AsReadOnly();

        //Disk
        private List<BaseDisk> _serverDisks = new List<BaseDisk>();
        public ICollection<BaseDisk> ServerDisks => _serverDisks.AsReadOnly();


        #region Managed disks

        public void AddOrUpdateWindowsDisk(string name, int size, char DriveLetter, string label = "")
        {

            var existingDisk = _serverDisks.FirstOrDefault(d => (d as WindowsDisk).Letter == DriveLetter);
            if (null == existingDisk)
            {
                var newDisk = new WindowsDisk(name, size, DriveLetter, label);
                _serverDisks.Add(newDisk);
            }
            else
            {
                existingDisk.SetSize(size);
                if (!String.IsNullOrWhiteSpace(label))
                {
                    ((WindowsDisk)existingDisk).SetLabel(label);
                }
            }

        }

        public void AddOrUpdateLinuxDisk(string name, int size, string datavg, string path)
        {

            var existingDisk = _serverDisks.FirstOrDefault(d => String.Compare((d as LinuxDisk).Path, path, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (null == existingDisk)
            {
                var newDisk = new LinuxDisk(name, size, datavg, path);
                _serverDisks.Add(newDisk);
            }
            else
            {
                existingDisk.SetSize(size);
                ((LinuxDisk)existingDisk).SetDatavg(datavg);
                ((LinuxDisk)existingDisk).SetPath(path);

            }

        }


        #endregion

        #region Managed environments

        public void LinkToEnvironment(Environment environment)
        {
            // Server can belong only to one Production Server and server can't be linked to Production and non production Server
            var existingProdEnvironment = _serverEnvironments.FirstOrDefault(e => e.Environment.IsProduction);
            if (null != existingProdEnvironment)
            {
                if (environment.IsProduction)
                {
                    if (String.Compare(existingProdEnvironment.Environment.Name, environment.Name, StringComparison.InvariantCultureIgnoreCase) != 0)
                    {
                        throw new ArgumentException($"Unable to link {HostName} to a second production Environment {environment.Name}");
                    }
                }
                else
                {
                    throw new ArgumentException($"Unable to link production server {HostName} to a non production Environment {environment.Name}");
                }
            }

            var existingEnvironment = _serverEnvironments.FirstOrDefault(e => String.Compare(e.Environment.Name, environment.Name, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (null == existingEnvironment)
            {
                var serverEnvironment = new ServerEnvironment()
                {
                    Environment = environment,
                    Server = this
                };

                _serverEnvironments.Add(serverEnvironment);
            }
        }
        #endregion

        #region Get Ansible Variables

        public Dictionary<String, Object> GetAnsibleVariables()
        {
            var variables = new Dictionary<String, Object>();
            variables.Add("server_hostname", HostName.ToLower());
            variables.Add("server_cpu", CPU);
            variables.Add("server_ram", RAM);

            var diskArray = new Dictionary<String, Object>[ServerDisks.Count];
            for (Int16 i = 0; i < ServerDisks.Count; i++)
            {
                diskArray[i] = GetAnsibleDiskVariables(ServerDisks.ElementAt(i));
            }
            variables.Add("server_disks", diskArray);

            return variables;
        }

        public Dictionary<String, Object> GetAnsibleDiskVariables(BaseDisk disk)
        {
            var variables = new Dictionary<String, Object>();
            variables.Add("name", disk.Name);
            variables.Add("size", disk.Size);
            variables.Add("format", disk.Format);



            if (disk is WindowsDisk)
            {
                variables.Add("label", (disk as WindowsDisk).Label);
                variables.Add("letter", (disk as WindowsDisk).Letter);
            }

            if (disk is LinuxDisk)
            {
                variables.Add("path", (disk as LinuxDisk).Path);
            }

            return variables;
        }


        #endregion

    }
}
