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
using System.Windows.Shapes;

namespace GuitarHeroView
{
    /// <summary>
    /// VGamePage.xaml 的交互逻辑
    /// </summary>
    public partial class VGamePage : Window
    {
        public VGamePage()
        {
            InitializeComponent();
        }

        #region Private State
        private const int TimerResolution = 2;
        private const double MaxFramerate = 70;
        private const double MinFramerate = 15;
        private DateTime lastFrameDrawn = DateTime.MinValue;
        private DateTime predNextFrame = DateTime.MinValue;
        private double targetFramerate = MaxFramerate;
        private int frameCount;
        private bool runningGameThread;
        #endregion
    }
}
