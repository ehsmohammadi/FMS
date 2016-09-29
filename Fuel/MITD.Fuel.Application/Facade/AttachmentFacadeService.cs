using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MITD.Fuel.Domain.Model.Repositories;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.FacadeServices;

namespace MITD.Fuel.Application.Facade
{
    public class AttachmentFacadeService : IAttachmentFacadeService
   {

       private IAttachmentRepository _attachmentRepository;
       public AttachmentFacadeService(IAttachmentRepository attachmentRepository)
       {
           _attachmentRepository = attachmentRepository;
       }

       public void Upload(int rowId, byte[] attachmentContent, string attachmentName, string attachmentExt,
           long entityId,
           int entityType, System.Guid rowGUID)
       {
           _attachmentRepository.Upload( rowId,  attachmentContent,  attachmentName,  attachmentExt,
            entityId,
            entityType,  rowGUID);
       }

        public AttachmentDto Download(int id)
        {
            var res = _attachmentRepository.Download(id);

            return new AttachmentDto()
            {
                AttachmentContent=res.Item3,
                AttachmentName =res.Item1,
                Ext = res.Item2
            };
        }
   }
}
