using HospitalWaitAreaShow.HandleJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalWaitAreaShow.ViewModel
{
    public class FourthFloorViewModel :BaseViewModel
    {
        public FourthFloorViewModel() : base(FloorNameEnum.Fourth)
        {
            this.FloorName = "四楼门诊区";
        }
    }
}
