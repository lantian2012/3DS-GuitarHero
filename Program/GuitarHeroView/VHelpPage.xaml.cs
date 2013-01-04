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
using System.Windows.Media.Animation;

namespace GuitarHeroView
{
    /// <summary>
    /// VHelpPage.xaml 的交互逻辑
    /// </summary>
    public partial class VHelpPage : Page
    {
        public VHelpPage()
        {
            InitializeComponent();
        }


        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ThicknessAnimation ta = new ThicknessAnimation(new Thickness(-618, 0, 1003, 0), new Duration(TimeSpan.FromMilliseconds(350)));
            ExponentialEase ee = new ExponentialEase();
            ee.EasingMode = EasingMode.EaseOut;
            ta.EasingFunction = ee;
            ta.Completed += ta_Completed;
            this.OptGrid.BeginAnimation(MarginProperty, ta);
        }

        void ta_Completed(object sender, EventArgs e)
        {
            (sender as AnimationClock).Completed -= ta_Completed;
            NavigationService.Navigate(new Uri("VStartPage.xaml", UriKind.Relative));
            //throw new NotImplementedException();
        }
    }
}
