using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Domain.Model.Exceptions;
using Remotion.Linq.Parsing;

namespace MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate
{
    public class Segment
    {
        #region Prop
        public long Id { get; private set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public SegmentType SegmentType { get; set; }
        public byte[] TimeStamp { get; set; }
        //public int SegmentTypeId { get; set; }

        public virtual JournalEntry JournalEntry { get; set; }

        public long JournalEntryId { get; set; }

        #endregion

        #region Ctor

        public Segment()
        {

        }

        public Segment(long id, string name, string code, int segmentTypeId, long journalEntryId, FreeAccount freeAccount)
        {
            this.Id = id;
            this.Name = name;
            this.Code = code;
            this.SegmentType = SegmentType.FindSegmentType(segmentTypeId);
            if (segmentTypeId == 5)
            {
                if (freeAccount == null || freeAccount.Id == 0)
                    throw new BusinessRuleException("Invalid Segment");
            }
            JournalEntryId = journalEntryId;
        }

        
        public long? EffectiveFactorId { get; private set; }
        public virtual EffectiveFactor EffectiveFactor { get; private set; }

        public virtual FreeAccount FreeAccount { get; private set; }

        public long? FreeAccountId { get; set; }


        #endregion


    }
}
