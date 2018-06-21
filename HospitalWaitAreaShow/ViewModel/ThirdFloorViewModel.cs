using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalWaitAreaShow.ViewModel
{
    public class ThirdFloorViewModel : BaseViewModel
    {
        public ThirdFloorViewModel() :base(HandleJson.FloorNameEnum.Third)
        {
            this.FloorName = "三楼门诊区";

            
        }

    }
}
