using GalaSoft.MvvmLight.Threading;
using HospitalWaitAreaShow.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace HospitalWaitAreaShow
{
    public sealed class CommonHelper
    {
        public static event EventHandler<GetDataEventArgs> GetDataFromSqlEvent;

        private static DateTime _dateInfo;
        private static int _intervalTime = 2;
        private static readonly CommonHelper _singleton = new CommonHelper();
        private DispatcherTimer _timer = null;
        Random r = new Random();//just for test

        private List<FZDataInfo> currentBaseList;


        private CommonHelper()
        {
            GetParams();
            currentBaseList = new List<FZDataInfo>();
            //GetDataAndRaiseEvent();


            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, _intervalTime)
            };

            _timer.Tick += _timer_Tick;
            _timer.Start();
        }


        private void _timer_Tick(object sender, EventArgs e)
        {

            //test from 6/1 to 6/7
#if Test
            var tempTime = new DateTime(2018, 6, r.Next(1, 8));
            _dateInfo = tempTime;
#else

#endif
            this.currentBaseList.Clear();

            GetDataAndRaiseEvent();
        }

        private async void GetDataAndRaiseEvent()
        {
            await Task.Run(async() =>
            {
                this.currentBaseList =await GetBaseDataFromSqlAsync(_dateInfo);
            });

            
            if (currentBaseList == null)
            {
                LogHelper.logerror.Error("Cannot get data from sql server !!");
                return;
            }

            try
            {
                //Make sure that using main thread to raise the event
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    GetDataFromSqlEvent?.Invoke(this, new GetDataEventArgs(currentBaseList));
                });

                
            }
            catch (Exception ex)
            {

                LogHelper.logerror.Error(ex);
            }
        }



        private static void GetResult(Task<long> obj)
        {
            Console.WriteLine("From ContinueWith + " + obj.Result);
        }

        public static async Task<List<FZDataInfo>> GetBaseDataFromSqlAsync(DateTime? timeinfo)
        {
            //Get the Elapsed time
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //Thread.Sleep(5000);
            
            try
            {
                using (hospitalEntities fz = new hospitalEntities())
                {
                    var tempTime = timeinfo ?? _dateInfo;

                    var tempList = await fz.PATI_OUT_VISIT_FZ.Where(x => x.FZSJ >= tempTime)
                        .Select(p => new FZDataInfo
                        { FZKS = p.FZKS, expert_flag = p.expert_flag, CALLTIME = p.CALLTIME, FZSJ = p.FZSJ, HJBZ = p.HJBZ }).ToListAsync();


                    sw.Stop();
                    LogHelper.loginfo.InfoFormat("查看数据库消耗了{0}ms", sw.ElapsedMilliseconds);

                    return tempList;

                }
            }
            catch (Exception ex)
            {
                LogHelper.logerror.Error(ex);
                return null;
            }

        }

        private void GetParams()
        {
            var intervalval = ConfigurationManager.AppSettings["intervalTimeSecond"].ToString();
            if (!int.TryParse(intervalval, out _intervalTime))
            {
                MessageBox.Show("Please input right interval time in app.config", "Error");
            }
            var dateval = ConfigurationManager.AppSettings["DateInfo"].ToString();
            if ("now" == dateval.ToLower())
            {
                //begin from 00:00:00 every day
                _dateInfo = DateTime.Now.Date;
            }
            else
            {
                if (!DateTime.TryParse(dateval, out _dateInfo))
                {
                    MessageBox.Show("Please input right datetime formart in app.config", "Error");
                }
            }

        }

        public static int IntervalTime
        {
            get
            {
                return _intervalTime;
            }


        }
        public static DateTime DateInfo
        {
            get
            {
                return _dateInfo;
            }
        }

    }
}
