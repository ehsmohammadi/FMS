using MITD.Fuel.Domain.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow
{
    public class Workflow
    {
        public const string DEFAULT_NAME = "Default";

        public long Id { get; set; }

        public string Name { get; set; }

        public WorkflowEntities WorkflowEntity { get; set; }
    
        public long CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public Workflow()
        {
        }

        public Workflow(string name, WorkflowEntities workflowEntity, long companyId)
        {
            Name = name;
            WorkflowEntity = workflowEntity;
            CompanyId = companyId;
        }

        public Workflow(string name, WorkflowEntities workflowEntity, Company company)
        {
            Name = name;
            WorkflowEntity = workflowEntity;
            Company = company;
        }

        public Workflow(WorkflowEntities workflowEntities, long companyId)
            : this(DEFAULT_NAME, workflowEntities, companyId)
        {
        }

        public Workflow(WorkflowEntities workflowEntities, Company company)
            : this(DEFAULT_NAME, workflowEntities, company)
        {
        }
    }
}
