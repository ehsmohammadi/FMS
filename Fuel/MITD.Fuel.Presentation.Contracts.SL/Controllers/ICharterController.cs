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
using MITD.Fuel.Presentation.Contracts.Enums;

namespace MITD.Fuel.Presentation.Contracts.SL.Controllers
{
    public interface ICharterController
    {
        void ShowCharterInList();
        void AddCharterIn();
        void UpdateCharterIn(long id);
        void AddCharterInItem(CharterStateTypeEnum charterStateTypeEnum, long charterId, long charterItemId, long vesselId);
        void UpdateCharterInItem(CharterStateTypeEnum charterStateTypeEnum, long charterId, long charterItemId, long vesselId);

        void ShowCharterOutList();
        void AddCharterOut();
        void UpdateCharterOut(long id);
        void AddCharterOutItem(CharterStateTypeEnum charterStateTypeEnum, long charterId, long charterItemId, long vesselId);
        void UpdateCharterOutItem(CharterStateTypeEnum charterStateTypeEnum, long charterId, long charterItemId, long vesselId);
       
    }
}
