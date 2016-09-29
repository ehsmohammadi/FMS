namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class GoodPartyAssignment
    {
        public GoodPartyAssignment()
        {
        }

        public GoodPartyAssignment(long id, string description, long companyId, long goodId)
        {
            Id = id;
            Description = description;
            GoodId = goodId;
            CompanyId = companyId;
        }

        public long Id { get; set; }
        public string Description { get; set; }
        public long GoodId { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}