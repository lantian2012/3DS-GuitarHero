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
using System.Runtime.InteropServices;
using GuitarHeroModel;
using GuitarHeroController;
using System.Threading;

namespace GuitarHeroView
{
    /// <summary>
    /// VOptionPage.xaml 的交互逻辑
    /// </summary>
    public partial class VOptionPage : Page
    {
        private MOptions _MOptions;
        private int _difficulty;
        private int _lastSelected;
        private CKinect _cKinect;
        public VOptionPage()
        {
            InitializeComponent();
            this._MOptions = new MOptions();
            this._MOptions.loadOptionFile();
        }
        #region Page Related
        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            this._difficulty = _MOptions.Difficulty;
            this._lastSelected = _MOptions.LastSelected;
            this.setUIWithProperties();
            
        }
        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            this.getPropertiesFromUI();
            exitPage();
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            exitPage();
        }
        /// <summary>
        /// called when exit the page
        /// </summary>
        private void exitPage() {
            // to check if the kinect is working
            if (this._cKinect != null && this._cKinect.isStart)
            {
                this._cKinect.stop();
                this.myGameThread.Abort();
            }

            ThicknessAnimation ta = new ThicknessAnimation(new Thickness(-337, 0, 991, 0), new Duration(TimeSpan.FromMilliseconds(350)));
            ExponentialEase ee = new ExponentialEase();
            ee.EasingMode = EasingMode.EaseOut;
            ta.EasingFunction = ee;
            ta.Completed += taExitPage_Completed;
            this.OptGrid.BeginAnimation(MarginProperty, ta);
        }

        void taExitPage_Completed(object sender, EventArgs e)
        {
            (sender as AnimationClock).Completed -= taExitPage_Completed;
            NavigationService.Navigate(new Uri("VStartPage.xaml", UriKind.Relative));
            //throw new NotImplementedException();
        }
        /// <summary>
        /// set UI Controls' value from _MOptions
        /// </summary>
        private void setUIWithProperties() {
            switch (this._difficulty) {
                case 0: {
                    this.LbDifficulty.Content = "      Easy";
                    break;
                }
                case 1:
                    {
                        this.LbDifficulty.Content = "     Normal";
                        break;
                    }
                case 2:
                    {
                        this.LbDifficulty.Content = "      Hard";
                        break;
                    }
                default: {
                    this.LbDifficulty.Content = "     Normal";
                    break;
                }
            }
            this.LbMusicName.Content = this._MOptions.getMusicName(this._lastSelected);

            if (this._difficulty == 0) {
                this.BtnHardDecrease.IsEnabled = false;
                this.BtnHardIncrease.IsEnabled = true;
            }
            else if (this._difficulty == 2)
            {
                this.BtnHardIncrease.IsEnabled = false;
                this.BtnHardDecrease.IsEnabled = true;
            }
            else {
                this.BtnHardDecrease.IsEnabled = true;
                this.BtnHardIncrease.IsEnabled = true;
            }

            if (this._lastSelected == 0 && this._lastSelected != this._MOptions.NumberOfMusic - 1)
            {
                this.BtnMusicLeft.IsEnabled = false;
                this.BtnMusicRight.IsEnabled = true;
            }
            else if (this._lastSelected == this._MOptions.NumberOfMusic - 1 && this._lastSelected != 0)
            {
                this.BtnMusicRight.IsEnabled = false;
                this.BtnMusicLeft.IsEnabled = true;
            }
            else if (this._lastSelected == 0 && this._lastSelected == this._MOptions.NumberOfMusic - 1) {
                this.BtnMusicRight.IsEnabled = false;
                this.BtnMusicLeft.IsEnabled = false;
            }
            else
            {
                this.BtnMusicLeft.IsEnabled = true;
                this.BtnMusicRight.IsEnabled = true;
            }
        }
        /// <summary>
        /// save the UI Controls' value into the _MOptions
        /// </summary>
        private void getPropertiesFromUI(){
            _MOptions.saveOptionFile(this._difficulty,this._lastSelected);
        }
        #endregion

        #region Modify UI Properties
        private void BtnHardIncrease_Click(object sender, RoutedEventArgs e)
        {
            if (this._difficulty < 2)
            {
                this._difficulty++;
                this.setUIWithProperties();
            }
        }

        private void BtnHardDecrease_Click(object sender, RoutedEventArgs e)
        {
            if (this._difficulty > 0)
            {
                this._difficulty--;
                this.setUIWithProperties();
            }
        }

        private void BtnMusicLeft_Click(object sender, RoutedEventArgs e)
        {
            if (this._lastSelected > 0)
            {
                this._lastSelected--;
                this.setUIWithProperties();
            }
        }

        private void BtnMusicRight_Click(object sender, RoutedEventArgs e)
        {
            if (this._lastSelected < this._MOptions.NumberOfMusic - 1)
            {
                this._lastSelected++;
                this.setUIWithProperties();
            }
        }

        private void BtnTestKinect_Click(object sender, RoutedEventArgs e)
        {
            this.BtnTestKinect.Content = "Stop";
            this.BtnTestKinect.Click -= BtnTestKinect_Click;
            this.BtnTestKinect.Click += BtnStopTestKinect_Click;

            DoubleAnimation da = new DoubleAnimation(1.0, new Duration(TimeSpan.FromSeconds(1)));
            da.Completed += daShowShadow_Completed;
            this.lVShadow.BeginAnimation(OpacityProperty, da);
            this.rVShadow.BeginAnimation(OpacityProperty, da);
        }
        private void BtnStopTestKinect_Click(object sender, RoutedEventArgs e) {
            this.BtnTestKinect.Content = "Test";
            this.BtnTestKinect.Click -= BtnStopTestKinect_Click;
            this.BtnTestKinect.Click += BtnTestKinect_Click;
            this._cKinect.stop();
            this.myGameThread.Abort();

            DoubleAnimation da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(1)));
            this.lVShadow.BeginAnimation(OpacityProperty, da);
            this.rVShadow.BeginAnimation(OpacityProperty, da);
        }

        public CShadow _lShadow;
        public CShadow _rShadow;
        void daShowShadow_Completed(object sender, EventArgs e)
        {
            (sender as AnimationClock).Completed -= daShowShadow_Completed;
            this._lShadow = new CShadow(450, 400);
            this._rShadow = new CShadow(600, 400);
            this.lVShadow.DataContext = this._lShadow;
            this.rVShadow.DataContext = this._rShadow;

            int elevationAngle = int.Parse(this.TxtElevationAngle.Text);
            this._cKinect = new CKinect();
            this._cKinect.start();
            this._cKinect.ElevationAngle = elevationAngle;

            TimeBeginPeriod(TimerResolution);
            this.myGameThread = new Thread(this.GameThread);
            myGameThread.SetApartmentState(ApartmentState.STA);
            myGameThread.Start();
        }
        /// <summary>
        /// check if the shadow is in the border
        /// </summary>
        /// <param name="pos">position to be checked</param>
        /// <param name="low">low bound</param>
        /// <param name="high">high bound</param>
        /// <returns></returns>
        private double checkBorder(double pos, int low, int high)
        {
            if (pos < low)
            {
                return low;
            }
            else if (high < pos)
            {
                return high;
            }
            else
            {
                return pos;
            }
        }
        private void getPositionFromKinect()
        {
            if (this._cKinect.LeftTracked)
            {
                double[] pos = new double[2];
                pos[0] = this._cKinect.getLeftShadowX();
                pos[1] = this._cKinect.getLeftShadowY();

                pos[0] = (810 * pos[0] + 405);
                pos[1] = (748 - (748 * pos[1] + 374));
                pos[0] = checkBorder(pos[0], 312, 947);
                pos[1] = checkBorder(pos[1], 0, 686);
                this._lShadow.X = (int)(pos[0]);
                this._lShadow.Y = (int)(pos[1]);
            }
            if (this._cKinect.RightTracked)
            {
                double[] pos = new double[2];
                pos[0] = this._cKinect.getRightShadowX();
                pos[1] = this._cKinect.getRightShadowY();

                pos[0] = (810 * pos[0] + 405);
                pos[1] = (748 - (748 * pos[1] + 374));
                pos[0] = checkBorder(pos[0], 312, 947);
                pos[1] = checkBorder(pos[1], 0, 686);
                this._rShadow.X = (int)(pos[0]);
                this._rShadow.Y = (int)(pos[1]);
            }
        }
        #region Thread Related
        private Thread myGameThread;
        private const int TimerResolution = 2;  // ms
        private const int NumIntraFrames = 3;
        private const double MaxFramerate = 70;
        private const double MinFramerate = 15;
        private DateTime lastFrameDrawn = DateTime.MinValue;
        private DateTime predNextFrame = DateTime.MinValue;
        private double actualFrameTime;
        [DllImport("Winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern int TimeBeginPeriod(uint period);
        private double targetFramerate = MaxFramerate;
        private int frameCount;
        private bool runningGameThread;

        private void GameThread()
        {
            this.runningGameThread = true;
            this.predNextFrame = DateTime.Now;
            this.actualFrameTime = 1000.0 / this.targetFramerate;
            // Try to dispatch at as constant of a framerate as possible by sleeping just enough since
            // the last time we dispatched.
            while (this.runningGameThread)
            {
                // Calculate average framerate.  
                DateTime now = DateTime.Now;
                if (this.lastFrameDrawn == DateTime.MinValue)
                {
                    this.lastFrameDrawn = now;
                }

                double ms = now.Subtract(this.lastFrameDrawn).TotalMilliseconds;
                this.actualFrameTime = (this.actualFrameTime * 0.95) + (0.05 * ms);
                this.lastFrameDrawn = now;

                // Adjust target framerate down if we're not achieving that rate
                this.frameCount++;
                if ((this.frameCount % 100 == 0) && (1000.0 / this.actualFrameTime < this.targetFramerate * 0.92))
                {
                    this.targetFramerate = Math.Max(MinFramerate, (this.targetFramerate + (1000.0 / this.actualFrameTime)) / 2);
                }

                if (now > this.predNextFrame)
                {
                    this.predNextFrame = now;
                }
                else
                {
                    double milliseconds = this.predNextFrame.Subtract(now).TotalMilliseconds;
                    if (milliseconds >= TimerResolution)
                    {
                        Thread.Sleep((int)(milliseconds + 0.5));
                    }
                }

                this.predNextFrame += TimeSpan.FromMilliseconds(1000.0 / this.targetFramerate);

                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new Action<int>(this.HandleGameTimer), 0);
            }
        }
        private void HandleGameTimer(int param)
        {
            if (this._cKinect.Tracked)
            {
                this.getPositionFromKinect();
            }
        }
        #endregion
        #endregion
    }
}
