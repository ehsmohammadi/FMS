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
using MITD.Fuel.Presentation.Contracts.DTOs;
using MITD.Fuel.Presentation.Contracts.Enums;

namespace MITD.Fuel.Presentation.Contracts.SL.Controllers
{
    public interface IUserController
    {
        void ShowChangePassWord();
        void ShowUserList();
        void ShowGroupList();
        void ShowGroupView(UserGroupDto userGroupDto);
        void ShowGroupView(string partyName);
        void ShowUserView(UserDto user);
       void ShowManageUserAccessList(PartyDto partyDto, bool isgroup, long groupId);
    }
}
