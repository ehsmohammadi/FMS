using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Domain.Repository;


namespace MITD.Fuel.Domain.Model.Repositories
{
    public interface IAttachmentRepository : IRepository
    {
        void Upload(int rowId, byte[] attachmentContent, string attachmentName, string attachmentExt, long entityId,
            int entityType, System.Guid rowGUID);

        Tuple<string,string,byte[]> Download(int entityId);




    }
}                   