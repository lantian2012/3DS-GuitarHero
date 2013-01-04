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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GuitarHeroController;

namespace GuitarHeroView
{
    /// <summary>
    /// GameEndNotification.xaml 的交互逻辑
    /// </summary>
    public partial class GameEndNotification : UserControl
    {
        public GameEndNotification(int score)
        {
            InitializeComponent();
            _CRank = new CRank();
            _name = _CRank.loadName();
            _rank = _CRank.loadRank();
            panelNewRecord.Visibility = System.Windows.Visibility.Hidden;
            rectNewRecord.Visibility = System.Windows.Visibility.Hidden;
            btnConfirm.Visibility = System.Windows.Visibility.Hidden;
            btnConfirm_Copy.Visibility = System.Windows.Visibility.Hidden;
            labRecords.Visibility = System.Windows.Visibility.Hidden;
            _score = score;
            txtName.Tag = false;
        }
        private CRank _CRank;
        private string[] _name;
        private int[] _rank;
        private int _score;

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            ThicknessAnimation ta = new ThicknessAnimation(new Thickness(250, 250, 250, 250), new Thickness(0, 0, 0, 0), new Duration(TimeSpan.FromMilliseconds(400)));
            ExponentialEase ee = new ExponentialEase();
            ee.EasingMode = EasingMode.EaseOut;
            ta.EasingFunction = ee;
            ta.Completed += taStart_Completed;
            this.BackRect.BeginAnimation(MarginProperty, ta);
        }

        private void taStart_Completed(object sender, EventArgs e)
        {
            btnConfirm_Copy.Visibility = System.Windows.Visibility.Visible;
            labRecords.Visibility = System.Windows.Visibility.Visible;
            if (_score > _rank[4])
            {
                panelNewRecord.Visibility = System.Windows.Visibility.Visible;
                rectNewRecord.Visibility = System.Windows.Visibility.Visible;
                btnConfirm.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                name1.Content = _name[0];
                name2.Content = _name[1];
                name3.Content = _name[2];
                name4.Content = _name[3];
                name5.Content = _name[4];
                score1.Content = _rank[0];
                score2.Content = _rank[1];
                score3.Content = _rank[2];
                score4.Content = _rank[3];
                score5.Content = _rank[4];
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "Your Name Here")
            {
                _CRank.updateRank("Anonymous", _score);
            }
            else
                _CRank.updateRank(txtName.Text, _score);
            _name = _CRank.loadName();
            _rank = _CRank.loadRank();
            name1.Content = _name[0];
            name2.Content = _name[1];
            name3.Content = _name[2];
            name4.Content = _name[3];
            name5.Content = _name[4];
            score1.Content = _rank[0];
            score2.Content = _rank[1];
            score3.Content = _rank[2];
            score4.Content = _rank[3];
            score5.Content = _rank[4];
            panelNewRecord.Visibility = System.Windows.Visibility.Collapsed;
            rectNewRecord.Visibility = System.Windows.Visibility.Collapsed;
            btnConfirm.Visibility = System.Windows.Visibility.Collapsed;
            

        }

        private void btnConfirm_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (panelNewRecord.Visibility == System.Windows.Visibility.Visible)
            {
                _CRank.updateRank("Anonymous", _score);
            }
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void txtName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((bool)txtName.Tag)
                txtName.SelectAll();
            txtName.Tag = false;
        }

        private void txtName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtName.Tag = true;
            txtName.SelectAll();
        }
    }
}
