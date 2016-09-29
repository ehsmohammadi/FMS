using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MITD.Fuel.Presentation.Contracts.SL.Controllers
{
    public interface IVoucherSetingController
    {
        void ShowVoucherSeting();

        void ShowLookUpAccount(int typ);

        void ShowAddVoucherSeting();

        void ShowUpdateVoucherSeting(long id);


        void ShowAddVoucherSetingDetail(long id);

        void ShowUpdateVoucherSetingDetail(long id,long detailId);
    }
}
