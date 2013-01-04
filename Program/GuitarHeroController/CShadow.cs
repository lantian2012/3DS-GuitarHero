using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GuitarHeroController
{
    public class CShadow: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private int _X;
        public int X
        {
            get { return _X; }
            set
            {
                _X = value;
                NotifyPropertyChanged("X");
            }
        }
        private int _Y;
        public int Y
        {
            get { return _Y; }
            set
            {
                _Y = value;
                NotifyPropertyChanged("Y");
            }
        }
        public CShadow() { }
        public CShadow(int cX, int cY) {
            this._X = cX;
            this._Y = cY;
        }
    }
}
