using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalWaitAreaShow.Model
{
    public class WaitAreaInfo : INotifyPropertyChanged
    {
        private string _areaName;
        private int _waitCount;

        public string AreaName
        {
            get
            {
                return _areaName;
            }
            set
            {
                if (_areaName != value)
                {
                    _areaName = value;
                    RaisePropertyChanged("AreaName");
                }
            }
        }

        public int WaitCount
        {
            get
            {
                return _waitCount;
            }
            set
            {
                if (_waitCount != value)
                {
                    _waitCount = value;
                    RaisePropertyChanged("WaitCount");
                }
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }        

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
