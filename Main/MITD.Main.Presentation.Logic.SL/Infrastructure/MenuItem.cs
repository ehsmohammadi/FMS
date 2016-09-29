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

namespace MITD.Main.Presentation.Logic.SL.Infrastructure
{
    public class MenuItem<T>
    {
        public MenuItem(string title,string methodName)
        {
            Title = title;
            MethodName = methodName;
        }
        
        public string Title { get; set; }
        public string MethodName { get; set; }
        public T Controller { get; set; }
    }
}
