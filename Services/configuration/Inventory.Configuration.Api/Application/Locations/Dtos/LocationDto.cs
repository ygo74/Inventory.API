using Inventory.Common.Application.Dto;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using System.Linq;

namespace Inventory.Configuration.Api.Application.Locations.Dtos
{
    public class LocationDto : ConfigurationEntityDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string CountryCode { get; set; }
        public string CityCode { get; set; }
        public string RegionCode { get; set; }

        public static Expression<Func<Location, LocationDto>> Projection
        {
            get
            {
                return e => new LocationDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    CountryCode = e.CountryCode,
                    CityCode = e.CityCode,
                    RegionCode = e.RegionCode,
                    CreatedBy = e.CreatedBy,
                    Created = e.Created,
                    LastModifiedBy = e.LastModifiedBy,
                    LastModified = e.LastModified,
                    Deprecated = e.Deprecated,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate                    
                };
            }
        }

    }

    public class LocationDatacenterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DatacenterType { get; set; }

        public static Expression<Func<Datacenter, LocationDatacenterDto>> Projection
        {
            get
            {
                return e => new LocationDatacenterDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    DatacenterType = e.DataCenterType.ToString()
                };
            }
        }
    }

    


    public class LocationWithDatacentersDto : LocationDto
    {
        public IEnumerable<LocationDatacenterDto> Datacenters { get; set; }

        public new static Expression<Func<Location, LocationWithDatacentersDto>> Projection
        {
            get
            {
                return e => new LocationWithDatacentersDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    CountryCode = e.CountryCode,
                    CityCode = e.CityCode,
                    RegionCode = e.RegionCode,
                    CreatedBy = e.CreatedBy,
                    Created = e.Created,
                    LastModifiedBy = e.LastModifiedBy,
                    LastModified = e.LastModified,
                    Deprecated = e.Deprecated,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Datacenters = e.Datacenters.AsQueryable().Select(LocationDatacenterDto.Projection).ToList()
                };
            }
        }
    }


    public static class LocationExtensions
    {
        private static Func<Location, LocationWithDatacentersDto> _toLocationWithDatacentersDto = LocationWithDatacentersDto.Projection.Compile();
        private static Func<Datacenter, LocationDatacenterDto> _toLocationDatacenterDto = LocationDatacenterDto.Projection.Compile();
        private static Func<Location, LocationDto> _toLocationDto = LocationDto.Projection.Compile();

        public static LocationDto ToLocationDto(this Location location)
        {
            return _toLocationDto(location);
        }

        public static LocationDatacenterDto ToLocationDatacenterDto(this Datacenter datacenter)
        {
            return _toLocationDatacenterDto(datacenter);
        }

        public static LocationWithDatacentersDto ToLocationWithDatacentersDto(this Location location)
        {
            return _toLocationWithDatacentersDto(location);
        }

    }




}
