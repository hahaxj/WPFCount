using HospitalWaitAreaShow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalWaitAreaShow
{
    public class GetDataEventArgs : EventArgs
    {
        public GetDataEventArgs(List<FZDataInfo> _dataList)
        {
            FzInfoList = _dataList;
        }

        public List<FZDataInfo> FzInfoList { get; private set; }
    }
}
