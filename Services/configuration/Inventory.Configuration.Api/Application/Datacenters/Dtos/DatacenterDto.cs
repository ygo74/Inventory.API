using Inventory.Common.Application.Dto;
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

        public static Expression<Func<Datacenter, DatacenterDto>> Projection
        {
            get
            {
                return d => new DatacenterDto
                {
                    Id = d.Id,
                    Code = d.Code,
                    Name = d.Name,
                    Description = d.Description,
                    DatacenterType = d.DataCenterType.ToString(),
                    LocationName = d.Location.Name,
                    LocationCountryCode = d.Location.CountryCode,
                    LocationCityCode = d.Location.CityCode,
                    LocationRegionCode = d.Location.RegionCode,
                    CreatedBy = d.CreatedBy,
                    Created = d.Created,
                    LastModifiedBy = d.LastModifiedBy,
                    LastModified = d.LastModified,
                    Deprecated = d.Deprecated,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate
                };
            }
        }
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
                    DatacenterId = d.Id,
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

        public static IEnumerable<DatacenterPluginsDto> GetFromDatacenter(Datacenter datacenter)
        {
            return datacenter.Plugins.Select(p => new DatacenterPluginsDto 
            {
                Id = p.Id,
                DatacenterId = datacenter.Id,
                CredentialName = p.Credential != null ? p.Credential.Name : null,
                CredentialDescription = p.Credential != null ? p.Credential.Description : null,
                CredentialPropertyBag = p.Credential != null ? p.Credential.PropertyBag : null,
                PluginName = p.Plugin != null ? p.Plugin.Name : null,
                PluginCode = p.Plugin != null ? p.Plugin.Code : null,
                PluginVersion = p.Plugin != null ? p.Plugin.Version : null,
                PluginPath = p.Plugin != null ? p.Plugin.Path : null,
                PluginEndpointPropertyBag = p.Plugin != null ? p.PropertyBag : null
            });

        }

    }
}
