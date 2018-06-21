using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HospitalWaitAreaShow.View
{
    /// <summary>
    /// Interaction logic for BaseWaitAreaView.xaml
    /// </summary>
    public class BaseWaitAreaView : UserControl
    {
        public BaseWaitAreaView()
        {
            InitializeComponent();
        }

        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("View/Base/BaseWaitAreaView.xaml", System.UriKind.Relative);

            var root = Application.LoadComponent(resourceLocater);
            this.Content = root;
            //使用FindName方法获取实例
            //this.BtnMe = ((System.Windows.Controls.Button)(((Grid)root).FindName("BtnMe")));
        }
        private bool _contentLoaded;
    }
}
