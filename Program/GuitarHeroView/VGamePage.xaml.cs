using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Timers;
using GuitarHeroController;
using System.Windows.Media.Animation;
using GuitarHeroModel;

namespace GuitarHeroView
{
    /// <summary>
    /// VGamePage.xaml 的交互逻辑
    /// </summary>
    public partial class VGamePage : Page
    {
        public VGamePage()
        {
            InitializeComponent();
        }
        #region Game Initialize
        private void PageTransition_Completed(object sender, EventArgs e)
        {
            this.LbHint.DataContext = this;

            initializeGameLogic();
            initializeGameField(3);

            showReadyHint();
        }

        private void initializeGameField(int numberOfStrings)
        {
            for (int i = 0; i < numberOfStrings; i++)
            {
                GameBoard.ColumnDefinitions.Add(new ColumnDefinition());
            }
            this.lVShadow.DataContext = this._Clogic.LeftShadow;
            this.rVShadow.DataContext = this._Clogic.RightShadow;
        }
        /// <summary>
        /// Initialize _CKinect and _CLogic.
        /// Game will not start in this function.
        /// </summary>
        private void initializeGameLogic() {
            initializeKinect();
            MOptions moption = new MOptions();
            moption.loadOptionFile();
            this._Clogic = new CLogic(moption);
        }
        private void initializeKinect()
        {
            this._CKinect = new CKinect();
            this._CKinect.start();
        }
        #endregion
        #region Game GetReady
        private int _readyCount;
        /// <summary>
        /// Show the Game GetReady process
        /// </summary>
        private void showReadyHint()
        {
            DoubleAnimation da = new DoubleAnimation(0.8, new Duration(TimeSpan.FromMilliseconds(800)));
            da.Completed += daShowHint_Completed;
            this.LbHint.BeginAnimation(OpacityProperty, da);
            this.lVShadow.BeginAnimation(OpacityProperty, da);
            this.rVShadow.BeginAnimation(OpacityProperty, da);
        }

        void daShowHint_Completed(object sender, EventArgs e)
        {
            (sender as AnimationClock).Completed -= daShowHint_Completed;
            this._readyCount = 0;
            TimeBeginPeriod(TimerResolution);
            myGameThread = new Thread(this.BeforeGameThread);
            myGameThread.SetApartmentState(ApartmentState.STA);
            myGameThread.Start();
        }

        private bool isInReady()
        {
            if ((200 < this._Clogic.LeftShadow.X)
                && (this._Clogic.LeftShadow.X < 615)
                && (308 < this._Clogic.LeftShadow.Y)
                && (this._Clogic.LeftShadow.Y < 423)
                && (200 < this._Clogic.RightShadow.X)
                && (this._Clogic.RightShadow.X < 615)
                && (308 < this._Clogic.RightShadow.Y)
                && (this._Clogic.RightShadow.Y < 423))
            {
                return true;
            }
            return false;
        }
        #endregion
        #region Game Start
        /// <summary>
        /// An event handler for Game Start;
        /// Called when the Game Start Process is beginning.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void daGameStart_Completed(object sender, EventArgs e)
        {
            this.InfoPanel.DataContext = this._Clogic;
            this._Clogic.startGame();

            (sender as AnimationClock).Completed -= daGameStart_Completed;
            TimeBeginPeriod(TimerResolution);
            myGameThread = new Thread(this.GameThread);
            myGameThread.SetApartmentState(ApartmentState.STA);
            myGameThread.Start();
        }
        #endregion

        #region Attributes
        private CKinect _CKinect;
        private CLogic _Clogic;
        #endregion

        #region Graphics Operations
        private void addNote()
        {
            int StringNumber = _Clogic.checkNewNote();
            if (StringNumber >= 0)
            {
                VNote note = new VNote();
                note.DataContext = _Clogic.addNewNote(StringNumber);
                Grid.SetZIndex(note, 3);
                Grid.SetColumn(note, StringNumber);
                GameBoard.Children.Add(note);
            }
        }
        #endregion

        #region Logic Related
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
            if (this._CKinect.LeftTracked) {
                double[] pos = new double[2];
                pos[0] = this._CKinect.getLeftShadowX();
                pos[1] = this._CKinect.getLeftShadowY();

                pos[0] = (810 * pos[0] + 405);
                pos[1] = (748 - (748 * pos[1] + 374));
                pos[0] = checkBorder(pos[0], 0, 747);
                pos[1] = checkBorder(pos[1], 0, 685);
                this._Clogic.LeftShadow.X = (int)(pos[0]);
                this._Clogic.LeftShadow.Y = (int)(pos[1]);
            }
            if (this._CKinect.RightTracked) {
                double[] pos = new double[2];
                pos[0] = this._CKinect.getRightShadowX();
                pos[1] = this._CKinect.getRightShadowY();

                pos[0] = (810 * pos[0] + 405);
                pos[1] = (748 - (748 * pos[1] + 374));
                pos[0] = checkBorder(pos[0], 0, 747);
                pos[1] = checkBorder(pos[1], 0, 685);
                this._Clogic.RightShadow.X = (int)(pos[0]);
                this._Clogic.RightShadow.Y = (int)(pos[1]);
            }
        }
        #endregion

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
        #region Thread Game GetReady
        private void BeforeGameThread() {
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

                this.Dispatcher.Invoke(DispatcherPriority.Send, new Action<int>(this.BeforeGameTimer), 0);
            }
        }

        private void BeforeGameTimer(int param)
        {
            if (this._CKinect.Tracked)
            {
                this.getPositionFromKinect();

                if (this.isInReady())
                {
                    if (this._readyCount % 70 == 0)
                    {
                        int ct = 3 - this._readyCount / 70;
                        this.LbHint.Content = "  Start in " + ct.ToString();
                    }
                    this._readyCount++;
                }
                else {
                    this._readyCount = 0;
                    this.LbHint.Content = " Get Ready";
                }

                if (this._readyCount >= 209)
                {
                    this.myGameThread.Abort();
                    DoubleAnimation da = new DoubleAnimation(0.0, new Duration(TimeSpan.FromMilliseconds(500)));
                    da.Completed += daGameStart_Completed;
                    this.LbHint.BeginAnimation(OpacityProperty, da);
                }
            }
            else
            {
                this.LbHint.Content = " Untracked";
                this._readyCount = 0;
            }
        }
        #endregion
        #region Thread Game Processing
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

                this.Dispatcher.Invoke(DispatcherPriority.Send, new Action<int>(this.HandleGameTimer), 0);
            }
        }

        private void HandleGameTimer(int param)
        {
            if ((this.frameCount % 100) == 0)
            {
                this._Clogic.setFrameRate((int)(1000.0 / this.actualFrameTime));
            }
            if (this._CKinect.Tracked)
            {
                this.getPositionFromKinect();
            }
            _Clogic.checkString();
            _Clogic.advanceFrame();
            addNote();
            if (_Clogic.checkGameOver())
            {
                myGameThread.Abort();
                GameEndNotification gen = new GameEndNotification(_Clogic.Score);
                gen.IsVisibleChanged += GEN_IsVisibleChanged;
                Canvas.SetLeft(gen, 250);
                Canvas.SetBottom(gen, 130);
                canv.Children.Add(gen);
            }
        }

        private void GEN_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool leavePage = false;
            if (((UserControl)sender).Visibility == System.Windows.Visibility.Collapsed)
            {
                leavePage = true;
            }
            if (leavePage) {
                if (this._CKinect.isStart) {
                    this._CKinect.stop();
                }
                ThicknessAnimation ta = new ThicknessAnimation(new Thickness(-1000, 0, 1000, 0), new Duration(TimeSpan.FromMilliseconds(900)));
                QuadraticEase qe = new QuadraticEase();
                qe.EasingMode = EasingMode.EaseOut;
                ta.EasingFunction = qe;
                ta.Completed += taExitPage_Completed;
                this.GameBoard.BeginAnimation(MarginProperty, ta);
            }
        }

        void taExitPage_Completed(object sender, EventArgs e)
        {
            (sender as AnimationClock).Completed -= taExitPage_Completed;
            NavigationService.Navigate(new Uri("VStartPage.xaml", UriKind.Relative));
            
        }
        #endregion
        #endregion

        private void Page_Unloaded_1(object sender, RoutedEventArgs e)
        {
            if (this._CKinect != null && this._CKinect.isStart)
            {
                this._CKinect.stop();
                this.myGameThread.Abort();
            }
        }
    }

}
