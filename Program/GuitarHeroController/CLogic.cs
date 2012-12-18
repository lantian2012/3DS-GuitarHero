using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GuitarHeroController
{

    public class CLogic : INotifyPropertyChanged
    {
        #region Attributes
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private CNote _CNote;
        private CShadow _CShadow;
        private MusicHit _MusicHit;
        private bool isGameOver;
        public bool isGameOver { get; set;}
        public bool isScorable { get; set;}
        private int _Score;
        public int Score
        {
            get { return _Score; }
            set
            {
                _Score = value;
                NotifyPropertyChanged("Score");
            }
        }
        private int _Progress;
        public int Progress
        {
            get { return _Progress; }
            set
            {
                _Progress = value;
                NotifyPropertyChanged("Progress");
            }
        }

        #endregion

        
    }
}
