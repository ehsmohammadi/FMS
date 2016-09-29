namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class SharedGood
    {
        public SharedGood()
        {
        }

        public SharedGood(long id, string name, string code, long mainUnitId)
        {
            Id = id;
            Name = name;
            Code = code;

            MainUnitId = mainUnitId;
           
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        
        public long MainUnitId { get; set; }


        public virtual Unit MainUnit { get; set; }
    }
}