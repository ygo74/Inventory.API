﻿using Inventory.Common.Application.Dto;
using Inventory.Configuration.Api.Application.Locations;
using Inventory.Configuration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Datacenters.Dtos
{
    public class DatacenterDto : ConfigurationEntityDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DatacenterType { get; set; }
        public string LocationName { get; set; }
        public string LocationCountryCode { get; set; }
        public string LocationCityCode { get; set; }
        public string LocationRegionCode { get; set; }
    }

    public enum DatacenterTypeDto
    {
        Cloud,
        OnPremise
    }

    public class  DatacenterPluginsDto
    {
        public int Id { get; set; }
        public int DatacenterId { get; set; }
        public string CredentialName { get; set; }
        public string CredentialDescription { get; set; }
        public Dictionary<string, Object> CredentialPropertyBag { get; set; }
        public string PluginName { get; set; }
        public string PluginCode { get; set; }
        public string PluginVersion { get; set; }
        public string PluginPath { get; set; }
        public Dictionary<string, Object> PluginEndpointPropertyBag { get; set; }

        public static Expression<Func<Datacenter, IEnumerable<DatacenterPluginsDto>>> Projection
        {
            get
            {
                return d => d.Plugins.Select(p => new DatacenterPluginsDto
                {
                    Id = p.Id,
                    DatacenterId = p.Id,
                    CredentialName = p.Credential.Name,
                    CredentialDescription = p.Credential.Description,
                    CredentialPropertyBag = p.Credential.PropertyBag,
                    PluginName = p.Plugin.Name,
                    PluginCode = p.Plugin.Code,
                    PluginVersion = p.Plugin.Version,
                    PluginPath = p.Plugin.Path,
                    PluginEndpointPropertyBag = p.PropertyBag
                });
            }
        }

    }
}
