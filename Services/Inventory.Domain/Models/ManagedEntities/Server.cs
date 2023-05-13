using Inventory.Common.DomainModels;
using Inventory.Domain.Enums;
using Inventory.Domain.Events;
using Inventory.Domain.Models.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Environment = Inventory.Domain.Models.Configuration.Environment;
using OperatingSystem = Inventory.Domain.Models.Configuration.OperatingSystem;

namespace Inventory.Domain.Models.ManagedEntities
{
    public class Server : ManagedEntity
    {

        protected Server()
        {
        }

        public Server(String hostName, OperatingSystem operatingSystem, Environment environment, int cpu, int ram, System.Net.IPAddress subnet) : this()
        {
            HostName = !String.IsNullOrEmpty(hostName) ? hostName.ToLower() : throw new ArgumentNullException(nameof(hostName));
            OperatingSystem = operatingSystem ?? throw new ArgumentNullException(nameof(operatingSystem));
            Subnet = subnet ?? throw new ArgumentNullException(nameof(subnet));
            CPU = cpu > 0 ? cpu : throw new ArgumentNullException(nameof(cpu));
            RAM = ram > 0 ? ram : throw new ArgumentNullException(nameof(ram));
            Status = LifecycleStatus.New;


            // Link server to Environment
            // LinkToEnvironment(environment);

        }

        public int ServerId { get; private set; }
        public string HostName { get; private set; }

        public int CPU { get; private set; }
        public int RAM { get; private set; }
        public System.Net.IPAddress Subnet { get; private set; }

        // OS Familly
        public int OperatingSystemId { get; private set; }
        public OperatingSystem OperatingSystem { get; private set; }

        // Many to Many Applications
        private List<ApplicationInstance> _applications = new List<ApplicationInstance>();
        public ICollection<ApplicationInstance> Applications => _applications.AsReadOnly();

        // Location
        public int? LocationId { get; private set; }
        public Location Location { get; private set; }


        public void SetLifecycleStatus(LifecycleStatus status)
        {
            Status = status;
            AddDomainEvent(new ServerChangedEvent(Id, HostName));
        }

        #region Managed environments

        public void AddApplicationInstance(ApplicationInstance application)
        {
            _applications.Add(application);
        }

        //public void LinkToEnvironment(Environment environment)
        //{
        //    // Server can belong only to one Production Server and server can't be linked to Production and non production Server
        //    var existingProdEnvironment = _serverEnvironments.FirstOrDefault(e => e.Environment.EnvironmentFamilly == EnvironmentFamily.Production);
        //    if (null != existingProdEnvironment)
        //    {
        //        if (environment.EnvironmentFamilly == EnvironmentFamily.Production)
        //        {
        //            if (String.Compare(existingProdEnvironment.Environment.Name, environment.Name, StringComparison.InvariantCultureIgnoreCase) != 0)
        //            {
        //                throw new ArgumentException($"Unable to link {HostName} to a second production Environment {environment.Name}");
        //            }
        //        }
        //        else
        //        {
        //            throw new ArgumentException($"Unable to link production server {HostName} to a non production Environment {environment.Name}");
        //        }
        //    }

        //    var existingEnvironment = _serverEnvironments.FirstOrDefault(e => String.Compare(e.Environment.Name, environment.Name, StringComparison.InvariantCultureIgnoreCase) == 0);
        //    if (null == existingEnvironment)
        //    {
        //        var serverEnvironment = new ServerEnvironment()
        //        {
        //            Environment = environment,
        //            Server = this
        //        };

        //        _serverEnvironments.Add(serverEnvironment);
        //    }
        //}
        #endregion

    }
}
