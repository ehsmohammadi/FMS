namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class ActivityLocation
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public string CountryName { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}