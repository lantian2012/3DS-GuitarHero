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
using GuitarHeroController;
using System.Timers;

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
            this.ell.Fill = Brushes.Red;
        }
        private Timer aTimer;
        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            
            aTimer = new System.Timers.Timer(33);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //remember not to be disposed
            aTimer.Start();
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {

        }

        #region Attributes
        private CLogic _Clogic;
        #endregion


        #region Graphics Operations
        private void initializeGameField(int numberOfStrings)
        {
            for (int i = 0; i < numberOfStrings; i++)
            {
                GameBoard.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }
        private void addNote(int stringNumber)
        {
            VNote note = new VNote();
        }
        #endregion
    }

}
