/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:HospitalWaitAreaShow"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;


namespace HospitalWaitAreaShow.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private FirstFloorViewModel _firstFloorViewModel;

        private SecondFloorViewModel _secondFloorViewModel;

        private ThirdFloorViewModel _thirdFloorViewModel;

        private FourthFloorViewModel _fourthFloorViewModel;
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            _firstFloorViewModel = new FirstFloorViewModel();
            _secondFloorViewModel = new SecondFloorViewModel();
            _thirdFloorViewModel = new ThirdFloorViewModel();
            _fourthFloorViewModel = new FourthFloorViewModel();

        }


        public FirstFloorViewModel FirstFloorViewModel
        {
            get
            {
                return _firstFloorViewModel;
            }
        }

        public SecondFloorViewModel SecondFloorViewModel
        {
            get
            {
                return _secondFloorViewModel;
            }
        }

        public ThirdFloorViewModel ThirdFloorViewModel
        {
            get
            {
                return _thirdFloorViewModel;
            }
        }

        public FourthFloorViewModel FourthFloorViewModel
        {
            get
            {
                return _fourthFloorViewModel;
            }
        }

    }
}