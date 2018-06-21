using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace HospitalWaitAreaShow.Converters
{
    public class LabelColorConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //new GradientStop().Color 
            int temp;
            if (int.TryParse(value.ToString(),out temp))
            {
                if (temp < 0)
                {
                    LogHelper.logerror.ErrorFormat("得到的统计时间值是{0}，为负数请查看分诊表格中的calltime和fzsj字段", value);

                    return Brushes.Green;
                }

                if (temp < 60)
                {
                    return Brushes.Green;
                }

                if (temp >= 60 && temp < 90)
                {
                    return Brushes.Yellow;
                }

                if (temp >= 90)
                {
                    return Brushes.Red;
                }
              
            }
            else
            {
                LogHelper.logerror.ErrorFormat("得到的统计时间值是{0}，无法转换为整数",value);
            }

            return Brushes.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Windows.DependencyProperty.UnsetValue;
            //throw new NotImplementedException();
        }
    }
}
