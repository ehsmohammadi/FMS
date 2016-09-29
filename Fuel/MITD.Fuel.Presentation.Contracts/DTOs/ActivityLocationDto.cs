using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class ActivityLocationDto
    {
        private long id;
        private string code;
        private string name;
        private string abbreviation;
        private string countryName;
        private double? latitude;
        private double? longititude;

        public long Id
        {
            get { return this.id; }
            set { this.SetField(p => p.Id, ref this.id, value); }
        }

        public string Code
        {
            get { return this.code; }
            set { this.SetField(p => p.Code, ref this.code, value); }
        }

        public string Name
        {
            get { return this.name; }
            set { this.SetField(p => p.Name, ref this.name, value); }
        }

        public string Abbreviation
        {
            get { return this.abbreviation; }
            set { this.SetField(p => p.Abbreviation, ref this.abbreviation, value); }
        }

        public double? Latitude
        {
            get { return this.latitude; }
            set { this.SetField(p => p.Latitude, ref this.latitude, value); }
        }

        public double? Longititude
        {
            get { return this.longititude; }
            set { this.SetField(p => p.Longititude, ref this.longititude, value); }
        }

        public string CountryName
        {
            get { return this.countryName; }
            set { this.SetField(p => p.CountryName, ref this.countryName, value); }
        }
    }
}