using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalWaitAreaShow.HandleJson;
using System.Windows.Threading;
using System.Configuration;
using System.Windows;
using HospitalWaitAreaShow.Model;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using GalaSoft.MvvmLight.Threading;
using System.Data.Entity;
using System.Diagnostics;

namespace HospitalWaitAreaShow.ViewModel
{
    public class BaseViewModel : ViewModelBase
    {
        private int _WaitAvgTime;
        private int _normalWaitAvgTime;
        private int _exportWaitAvgTime;

        private AreaNameList _areaNameList;
        private WaitAreaCountList _normalWaitAreaCountList;
        private WaitAreaTimeList _normalWaitAreaTimeList;

        private WaitAreaCountList _exportWaitAreaCountList;
        private WaitAreaTimeList _exportWaitAreaTimeList;

        private string _floorName;

        private string _mostWaitKSInfo;

        /// <summary>
        /// key is KS code,value is KS name
        /// </summary>
        private Dictionary<string, string> _ksNameList;

        /// <summary>
        /// 每条记录是对应每个楼层的区域，每条记录的具体集合石对应每个区域的诊室
        /// </summary>
        private List<List<string>> _areaOfficeNameList;

        private SeriesCollection _seriesCollection;
        //public string[] Labels { get; set; }
        public Func<int, string> Formatter { get; set; }

        public BaseViewModel(FloorNameEnum floorName)
        {
            CommonHelper.GetDataFromSqlEvent += CommonHelper_GetDataFromSqlEvent;


            CurrentFloor = ParseFloor.FloorWaitAreaList
                .Where(x => x.FloorName == floorName).FirstOrDefault<FloorInfoList>();

            //FloorName = floorName.ToString() + " Floor";

            _areaNameList = new AreaNameList();
            _normalWaitAreaCountList = new WaitAreaCountList();
            _normalWaitAreaTimeList = new WaitAreaTimeList();
            _exportWaitAreaCountList = new WaitAreaCountList();
            _exportWaitAreaTimeList = new WaitAreaTimeList();
            _seriesCollection = new SeriesCollection();

            ///No need to set property
            _areaOfficeNameList = new List<List<string>>();

            Formatter = value => value.ToString("N");

            _ksNameList = new Dictionary<string, string>();

            InitAreaName();

            InitData();            
        }

        protected virtual void CommonHelper_GetDataFromSqlEvent(object sender, GetDataEventArgs e)
        {

            if (e.FzInfoList.Count == 0)
            {
                return;
            }

            //DispatcherHelper.CheckBeginInvokeOnUI(() =>
            //{
            //    ParseDataFromBase(e.FzInfoList);
            //    UpdateInterface();
            //});

            ParseDataFromBase(e.FzInfoList);
            UpdateInterface();

        }       

        /// <summary>
        /// Run once
        /// </summary>
        private void InitAreaName()
        {
            this.AreaNameList.Clear();
            this._areaOfficeNameList.Clear();

            foreach (var area in CurrentFloor.WaitAreaInfo)
            {
                this.AreaNameList.Add(area.Name);
                List<string> tempList = area.OfficeList.Select(x => x.Code).ToList();
                _areaOfficeNameList.Add(tempList);

                try
                {
                    foreach (var item in area.OfficeList)
                    {
                        this._ksNameList.Add(item.Code, item.Name);
                    }
                }
                catch (Exception ex)
                {

                    LogHelper.logerror.Error(ex);
                }


            }
        }

        /// <summary>
        /// Run once
        /// </summary>
        private async void InitData()
        {
            List<FZDataInfo> datalist = null;

            await Task.Run(async () =>
            {

                datalist = await CommonHelper.GetBaseDataFromSqlAsync(CommonHelper.DateInfo);
            });

            ParseDataFromBase(datalist);

            //GetData(_dateInfo);

            UpdateInterface();


        }

        private void ParseDataFromBase(List<FZDataInfo> currentDataList)
        {
            ClearList();

            if (currentDataList == null || currentDataList.Count == 0)
            {
                LogHelper.logerror.Error("Cannot get data from sql server");
                return;
            }

            try
            {
                foreach (var areaList in this._areaOfficeNameList)
                {
#if Test

                    var waitCountforOneArea = currentDataList.Where(x =>
                             x.HJBZ == "2" && areaList.Any(a => a == x.FZKS))
                            .Select(p => new { p.FZKS, p.expert_flag })
                            .ToList();
#else
                    var waitCountforOneArea = currentDataList.Where(x =>
                             x.HJBZ == "0" && areaList.Any(a => a == x.FZKS))
                            .Select(p => new { p.FZKS, p.expert_flag })
                            .ToList();
#endif

                    var queryMostCount = waitCountforOneArea
                                 .GroupBy(x => new { x.FZKS, x.expert_flag })
                                 .Select(group => new
                                 {
                                     Key = group.Key,
                                     Count = group.Count()
                                 });

                    var ksinfo = queryMostCount.OrderByDescending(x => x.Count).FirstOrDefault();

                    MostWaitKSInfo = string.Format("提示: \r\n{0} {1} \r\n等待人数 : {2}",
                        _ksNameList[ksinfo.Key.FZKS], ksinfo.Key.expert_flag == "0" ? "普通号" : "专家号", ksinfo.Count);

                    //Update Wait count for normal
                    NormalWaitAreaCountList.Add(waitCountforOneArea.Where(x => x.expert_flag == "0").Count());

                    //Update Wait count for export
                    ExportWaitAreaCountList.Add(waitCountforOneArea.Where(x => x.expert_flag != "0").Count());

                    var templistforTime = (from p in currentDataList
                                           where p.FZSJ.HasValue && p.CALLTIME.HasValue && areaList.Any(a => a == p.FZKS)
                                           select new { p.CALLTIME, p.FZSJ, p.expert_flag }).ToList();

                    var timeInfoforNormal = templistforTime.Where(x => x.expert_flag == "0").Select(x => (x.CALLTIME - x.FZSJ).Value.TotalMinutes).ToList();

                    //Update wait avg time for normal
                    if (timeInfoforNormal.Count > 0)
                        NormalWaitAreaTimeList.Add(Math.Round((double)timeInfoforNormal.Average(), 0));
                    else
                    {
                        NormalWaitAreaTimeList.Add(0);
                    }

                    var timeInfoforExport = templistforTime.Where(x => x.expert_flag != "0").Select(x => (x.CALLTIME - x.FZSJ).Value.TotalMinutes).ToList();


                    //Update wait avg time for export
                    if (timeInfoforExport.Count > 0)
                    {
                        ExportWaitAreaTimeList.Add(Math.Round((double)timeInfoforExport.Average(), 0));
                    }
                    else
                    {
                        ExportWaitAreaTimeList.Add(0);
                    }
                }

                NormalWaitAvgTime = (int)Math.Round(NormalWaitAreaTimeList.Average());
                ExportWaitAvgTime = (int)Math.Round(ExportWaitAreaTimeList.Average());
                WaitAvgTime = (NormalWaitAvgTime + ExportWaitAvgTime) / 2;

                LogHelper.loginfo.Info("Update data successfully!");
            }
            catch (Exception ex)
            {

                LogHelper.logerror.Error(ex);
            }
        }
       

        private void ClearList()
        {
            NormalWaitAreaCountList.Clear();
            NormalWaitAreaTimeList.Clear();
            ExportWaitAreaCountList.Clear();
            ExportWaitAreaTimeList.Clear();
        }

        private void UpdateInterface()
        {
            //SeriesCollection.Clear();

            if (SeriesCollection.Count == 0)
            {
                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = (string)Application.Current.FindResource("NormalTypeTitle"),//this._normalTitle,
                        Values = new ChartValues<int>(NormalWaitAreaCountList),
                        DataLabels = true
                    }
                };

                    //adding series will update and animate the chart automatically
                    SeriesCollection.Add(new ColumnSeries
                    {
                        Title = (string)Application.Current.FindResource("ExportTypeTitle"),//this._exportTitle,
                        Values = new ChartValues<int>(ExportWaitAreaCountList),
                        DataLabels = true
                    });
            }
            else
            {

                SeriesCollection[0].Values.Clear();
                SeriesCollection[1].Values.Clear();

                //SeriesCollection[0].Values = new ChartValues<int>(NormalWaitAreaCountList);
                SeriesCollection[0].Values.AddRange(NormalWaitAreaCountList.Cast<object>());
                
                //SeriesCollection[1].Values = new ChartValues<int>(ExportWaitAreaCountList);
                SeriesCollection[1].Values.AddRange(ExportWaitAreaCountList.Cast<object>());
            }
        }

        public FloorInfoList CurrentFloor { get; private set; }

        #region Change Porperty
        public int WaitAvgTime
        {
            get
            {
                return _WaitAvgTime;
            }
            set
            {
                _WaitAvgTime = value;
                RaisePropertyChanged("WaitAvgTime");
            }
        }

        public int NormalWaitAvgTime
        {
            get
            {
                return _normalWaitAvgTime;
            }
            set
            {
                _normalWaitAvgTime = value;
                RaisePropertyChanged("NormalWaitAvgTime");
            }
        }

        public int ExportWaitAvgTime
        {
            get
            {
                return _exportWaitAvgTime;
            }
            set
            {
                _exportWaitAvgTime = value;
                RaisePropertyChanged("ExportWaitAvgTime");
            }
        }

        public string FloorName
        {
            get
            {
                return _floorName;
            }
            set
            {
                if (value != _floorName)
                {
                    _floorName = value;
                    RaisePropertyChanged("FloorName");
                }

            }
        }

        public string MostWaitKSInfo
        {
            get
            {
                return _mostWaitKSInfo;
            }
            set
            {
                if (value != _mostWaitKSInfo)
                {
                    _mostWaitKSInfo = value;
                    RaisePropertyChanged("MostWaitKSInfo");
                }

            }
        }

        public AreaNameList AreaNameList
        {
            get
            {
                return _areaNameList;
            }
            set
            {
                _areaNameList = value;
                RaisePropertyChanged("AreaNameList");
            }
        }

        public WaitAreaCountList NormalWaitAreaCountList
        {
            get
            {
                return _normalWaitAreaCountList;
            }
            set
            {
                _normalWaitAreaCountList = value;
                RaisePropertyChanged("NormalWaitAreaCountList");
            }
        }

        public WaitAreaTimeList NormalWaitAreaTimeList
        {
            get
            {
                return _normalWaitAreaTimeList;
            }
            set
            {
                _normalWaitAreaTimeList = value;
                RaisePropertyChanged("NormalWaitAreaTimeList");
            }
        }

        public WaitAreaCountList ExportWaitAreaCountList
        {
            get
            {
                return _exportWaitAreaCountList;
            }
            set
            {
                _exportWaitAreaCountList = value;
                RaisePropertyChanged("ExportWaitAreaCountList");
            }
        }

        public WaitAreaTimeList ExportWaitAreaTimeList
        {
            get
            {
                return _exportWaitAreaTimeList;
            }
            set
            {
                _exportWaitAreaTimeList = value;
                RaisePropertyChanged("ExportWaitAreaTimeList");
            }
        }

        public SeriesCollection SeriesCollection
        {
            get
            {
                return _seriesCollection;
            }
            set
            {
                _seriesCollection = value;
                RaisePropertyChanged("SeriesCollection");
            }
        }
        #endregion



    }
}
