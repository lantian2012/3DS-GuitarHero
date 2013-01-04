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
using GuitarHeroModel;

namespace GuitarHeroView
{
    /// <summary>
    /// VStartPage.xaml 的交互逻辑
    /// </summary>
    public partial class VStartPage : Page
    {
        public VStartPage()
        {
            InitializeComponent();
            //MOptions a = new MOptions();
        }

        private void BtnGamePage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("VGamePage.xaml", UriKind.Relative));
        }

        private void BtnOptionPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("VOptionPage.xaml", UriKind.Relative));
        }

        private void BtnRankPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("VRankPage.xaml", UriKind.Relative));
        }
        private void BtnHelpPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("VHelpPage.xaml", UriKind.Relative));
        }
    }
}
