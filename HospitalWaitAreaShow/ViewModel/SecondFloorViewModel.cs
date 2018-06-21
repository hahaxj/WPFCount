using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalWaitAreaShow.ViewModel
{
    public class SecondFloorViewModel : BaseViewModel
    {
        public SecondFloorViewModel() :base(HandleJson.FloorNameEnum.Second)
        {
            this.FloorName = "二楼门诊区";
        }
    }
}
