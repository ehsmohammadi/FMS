using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.Enums;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class ApprovmentDto
    {
        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }
        long entityId;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "entity id  can't be empty")]
        public long EntityId
        {
            get { return entityId; }
            set { this.SetField(p => p.EntityId, ref entityId, value); }
        }

        long actorId;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "entity id  can't be empty")]
        public long ActorId
        {
            get { return actorId; }
            set { this.SetField(p => p.ActorId, ref actorId, value); }
        }
        string remark;
        public string Remark
        {
            get { return remark; }
            set { this.SetField(p => p.Remark, ref remark, value); }
        }

        ActionTypeEnum actionType;
        //  [Required(AllowEmptyStrings = false, ErrorMessage = "Action Type  can't be empty")]
        public ActionTypeEnum ActionType
        {
            get { return actionType; }
            set { this.SetField(p => p.ActionType, ref actionType, value); }
        }
        ActionEntityTypeEnum entityType;
        //  [Required(AllowEmptyStrings = false, ErrorMessage = "Action Type  can't be empty")]
        public ActionEntityTypeEnum ActionEntityType
        {
            get { return entityType; }
            set { this.SetField(p => p.ActionEntityType, ref entityType, value); }
        }

        DecisionTypeEnum decisionType;
        //  [Required(AllowEmptyStrings = false, ErrorMessage = "Desision Type  can't be empty")]
        public DecisionTypeEnum DecisionType
        {
            get { return decisionType; }
            set { this.SetField(p => p.DecisionType, ref decisionType, value); }
        }
    }
}
