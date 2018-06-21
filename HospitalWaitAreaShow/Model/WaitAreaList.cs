using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalWaitAreaShow.Model
{
    public class WaitAreaList : ObservableCollection<WaitAreaInfo>
    {
        
    }

    public class WaitAreaCountList : ObservableCollection<int>
    {

    }

    public class WaitAreaTimeList : ObservableCollection<double>
    {

    }

    public class AreaNameList : ObservableCollection<string>
    {

    }
}
