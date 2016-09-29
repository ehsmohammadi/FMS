using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MITD.Core;
using MITD.Presentation;
using MITD.Fuel.Presentation.Contracts.SL.Controllers;
using MITD.Presentation;
using MITD.StorageSpace.Presentation.Contracts.SL.Controllers;

namespace MITD.Main.Presentation.Logic.SL.Infrastructure
{
    public class MenuHelper
    {
        private string name;
        private IFuelController storageSpaceController;

        private ObservableCollection<CommandViewModel> cmdList;

        //private List<MenuItem<Object>> menuItems;

        //public List<MenuItem<Object>> MenuItems { get; set; }

        //public MenuHelper(string name,IStorageSpaceController storageSpaceController)
        //{
        //    this.storageSpaceController = storageSpaceController;
        //    this.name = name;
        //}

        public MenuHelper(IFuelController storageSpaceController)
        {
            

            this.storageSpaceController = storageSpaceController;
            this.cmdList = new ObservableCollection<CommandViewModel>();
            //if (MenuItems==null)
            //    MenuItems = new List<MenuItem<Object>>();
            //this.menuItems = MenuItems;
        }

        public ReadOnlyObservableCollection<CommandViewModel> ExecuteMenu()
        {


            return new ReadOnlyObservableCollection<CommandViewModel>(cmdList);


        }





        //private ObservableCollection<CommandViewModel> createMenu()
        //{
        //   // ObservableCollection<CommandViewModel> cmdList = new ObservableCollection<CommandViewModel>();

        //    foreach (MenuItem<Object> menuItem in menuItems)
        //    {

        //        string title = menuItem.Title;
        //        string methodName = menuItem.MethodName;

        //        cmdList.Add(new CommandViewModel(title, new DelegateCommand(
        //                                                                    () =>
        //                                                                    {


        //                                                                        storageSpaceController.ShowBusyIndicator(
        //                                                                            "در حال بارگذاری نوع انبار...");

        //                                                                        storageSpaceController.GetRemoteInstance<>(
        //                                                                            (res, exp) =>
        //                                                                            {
        //                                                                                storageSpaceController.HideBusyIndicator
        //                                                                                    ();
        //                                                                                if (res != null)
        //                                                                                {
        //                                                                                    res.GetType().GetMethod(methodName).
        //                                                                                        Invoke(res, new object[] { });
        //                                                                                }
        //                                                                                else if (exp != null)
        //                                                                                {
        //                                                                                    storageSpaceController.
        //                                                                                        HandleException(exp);
        //                                                                                }
        //                                                                            }



        //                                                                            );
        //                                                                    }

        //                                                                    )));
        //    }



        //    return cmdList;
        //}

        public void AddMenuItem<T>(MenuItem<T> menuItem) where T : class
        {
            cmdList.Add(new CommandViewModel(menuItem.Title, new DelegateCommand(
                                                             () =>
                                                             {
                                                                 storageSpaceController.ShowBusyIndicator(
                                                                     "در حال بارگذاری نوع انبار ...");

                                                                 storageSpaceController.GetRemoteInstance<T>(
                                                                     (res, exp) => storageSpaceController.
                                                                         BeginInvokeOnDispatcher(() =>
                                                                                                     {
                                                                                                         storageSpaceController.HideBusyIndicator();
                                                                                                         if (res != null)
                                                                                                         {
                                                                                                             res.GetType().GetMethod(menuItem.MethodName).
                                                                                                                 Invoke(res, new object[] { });
                                                                                                         }
                                                                                                         else if (exp != null)
                                                                                                         {
                                                                                                             storageSpaceController.HandleException(exp);
                                                                                                         }
                                                                                                     }));
                                                             }

                                                             )));
        }

        //private CommandViewModel GetInstance(string methodName)
        //{
        //    return  new CommandViewModel(this.name,new DelegateCommand(
        //        ()=>
        //            {


        //                storageSpaceController.ShowBusyIndicator("در حال بارگذاری نوع انبار...");
        //                storageSpaceController.GetRemoteInstance <T> (
        //                    (res, exp) =>
        //                        {
        //                            storageSpaceController.HideBusyIndicator();
        //                            if (res != null)
        //                            {
        //                                res.GetType().GetMethod(methodName).Invoke(res,new object[]{});
        //                            }
        //                            else if (exp != null)
        //                            {
        //                                storageSpaceController.HandleException(exp);
        //                            }
        //                        }



        //                    );
        //            }

        //                                               ));
        //}
    }
}
