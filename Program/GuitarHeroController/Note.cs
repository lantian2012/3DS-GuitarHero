using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GuitarHeroController
{
    public class Note : INotifyPropertyChanged
    {
        #region Attributs Defination
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
        public int speed { get; set; }
        private int _Height;
        public int Height
        {
            get { return _Height; }
            set
            {
                _Height = value;
                NotifyPropertyChanged("Height");
            }
        }
        private double _Opacity;
        public double Opacity
        {
            get { return _Opacity; }
            set
            {
                _Opacity = value;
                NotifyPropertyChanged("Opacity");
            }
        }
        private string _color;
        public string color
        {
            get { return _color; }
            set
            {
                _color = value;
                NotifyPropertyChanged("color");
            }
        }
        public bool isScorable { get; set; }
        public bool isAlive { get; set; }
        #endregion

        public Note(int s, string c)
        {
            Height = 100;
            Opacity = 1;
            X = 0;
            Y = 0;
            speed = s;
            color = c;
            isScorable = true;
            isAlive = true;
        }
        



    }
}
