﻿using System;
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
using GuitarHeroController;

namespace GuitarHeroView
{
    /// <summary>
    /// VRankPage.xaml 的交互逻辑
    /// </summary>
    public partial class VRankPage : Page
    {

        public VRankPage()
        {
            InitializeComponent();
            _CRank = new CRank();
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
        }
        private CRank _CRank;
        private string[] _name;
        private int[] _rank;
        
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ThicknessAnimation ta = new ThicknessAnimation(new Thickness(-337, 0, 991, 0), new Duration(TimeSpan.FromMilliseconds(350)));
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
