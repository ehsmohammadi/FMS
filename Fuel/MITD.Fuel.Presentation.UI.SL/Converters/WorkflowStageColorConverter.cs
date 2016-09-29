using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using MITD.Fuel.Presentation.Contracts.Enums;

namespace MITD.Fuel.Presentation.UI.SL.Converters
{
    public class WorkflowStageColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color foregroundColor = Colors.Black;

            if (!(value is WorkflowStageEnum))
                foregroundColor = Colors.Black;

            var workflowStage = (WorkflowStageEnum) value;

            switch (workflowStage)
            {
                case WorkflowStageEnum.FinalApproved:
                case WorkflowStageEnum.Submited:
                    foregroundColor = Color.FromArgb(255, 40, 150, 40);
                    //return Colors.Green;
                    break;

                case WorkflowStageEnum.Closed:
                    //return Colors.Brown;
                    foregroundColor = Color.FromArgb(255, 0, 120, 140);
                    break;
                
                case WorkflowStageEnum.Canceled:
                    foregroundColor = Colors.Brown;
                    break;
                
                case WorkflowStageEnum.SubmitRejected:
                    foregroundColor = Color.FromArgb(255, 255, 128, 64);
                    break;
                
                case WorkflowStageEnum.FinancialSubmitted:
                    //return Colors.Blue;
                    foregroundColor = Color.FromArgb(255, 0, 0, 180);
                    //Color.FromArgb(255, 0, 130, 160);
                    break;

                case WorkflowStageEnum.None:
                case WorkflowStageEnum.Initial:
                case WorkflowStageEnum.Approved:
                default:
                    foregroundColor = Colors.Black;
                    break;
            }

            return new SolidColorBrush( foregroundColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class WorkflowStageFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is WorkflowStageEnum))
                return FontWeights.Normal;

            var workflowStage = (WorkflowStageEnum)value;

            switch (workflowStage)
            {
                case WorkflowStageEnum.FinalApproved:
                case WorkflowStageEnum.Submited:
                case WorkflowStageEnum.Closed:
                case WorkflowStageEnum.Canceled:
                case WorkflowStageEnum.SubmitRejected:
                case WorkflowStageEnum.FinancialSubmitted:
                    return FontWeights.Bold;

                case WorkflowStageEnum.None:
                case WorkflowStageEnum.Initial:
                case WorkflowStageEnum.Approved:
                default:
                    return FontWeights.Normal;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
