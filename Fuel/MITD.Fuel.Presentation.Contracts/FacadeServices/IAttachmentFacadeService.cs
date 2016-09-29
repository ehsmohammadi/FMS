using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Services.Facade;

namespace MITD.Fuel.Presentation.Contracts.FacadeServices
{
  public  interface IAttachmentFacadeService:IFacadeService
  {
      void Upload(int rowId, byte[] attachmentContent, string attachmentName, string attachmentExt,
          long entityId,
          int entityType, System.Guid rowGUID);

      AttachmentDto Download(int id);
  }
}
