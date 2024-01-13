namespace Inventory.Configuration.Api.Application.Datacenters.Dtos
{
    public interface IDatacenterLocation
    {
        string CountryCode { get; set; }
        string CityCode { get; set; }
        string RegionCode { get; set; }
    }
}
