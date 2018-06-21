using System;
using System.Windows;
using System.Windows.Threading;


namespace HospitalWaitAreaShow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer = null;

        public MainWindow()
        {
            InitializeComponent();

            FullScreen();

            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };

            _timer.Tick += timer_Tick; ;
            _timer.Start();

            //log4net.ILog logger = LogHelper.loginfo;
            //try
            //{
            //    logger.Info("1111 " + "->" + DateTime.Now.ToLongTimeString());

            //}
            //catch (Exception ex)
            //{

            //    logger.Error(ex);
            //}
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.showTimeinfo.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
        }

        private void FullScreen()
        {
            this.WindowState = WindowState.Normal;
            this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            this.ResizeMode = System.Windows.ResizeMode.CanMinimize;
            this.Left = 0;
            this.Top = 0;
            this.Width = System.Windows.SystemParameters.VirtualScreenWidth;
            this.Height = System.Windows.SystemParameters.VirtualScreenHeight;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.showTimeinfo.Text = DateTime.Now.ToString("yyyy年-MM月dd日 HH:mm");
        }
    }
}
