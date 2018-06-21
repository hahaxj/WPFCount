using HospitalWaitAreaShow.HandleJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HospitalWaitAreaShow.ViewModel
{
    public class FirstFloorViewModel : BaseViewModel
    {
        
        public FirstFloorViewModel() : base(FloorNameEnum.First)
        {
            this.FloorName = "一楼门诊区";
        }
    }
}
