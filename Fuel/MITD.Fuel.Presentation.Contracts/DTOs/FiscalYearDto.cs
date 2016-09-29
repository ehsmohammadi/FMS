using System;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class FiscalYearDto
    {
        private int id;
        
        public int Id
        {
            get { return this.id; }
            set { this.SetField(p => p.Id, ref this.id, value); }
        }

        private int yearNumber;

        public int YearNumber
        {
            get { return this.yearNumber; }
            set { this.SetField(p => p.YearNumber, ref this.yearNumber, value); }
        }

        private string displayText;

        public string DisplayText
        {
            get { return this.displayText; }
            set { this.SetField(p => p.DisplayText, ref this.displayText, value); }
        }

        private DateTime fromDateTime;

        public DateTime FromDateTime
        {
            get { return this.fromDateTime; }
            set { this.SetField(p => p.FromDateTime, ref this.fromDateTime, value); }
        }


        private DateTime toDateTime;

        public DateTime ToDateTime
        {
            get { return this.toDateTime; }
            set { this.SetField(p => p.ToDateTime, ref this.toDateTime, value); }
        }
   }
}