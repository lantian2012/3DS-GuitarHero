using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GuitarHeroController
{
    class Note
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
        private int _Width;
        public int Width
        {
            get { return _Width; }
            set
            {
                _Width = value;
                NotifyPropertyChanged("Width");
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

        #endregion

        public Note(int s, string c)
        {
            Width = 125;
            Height = 70;
            X = (250 - Width) / 2;
            Y = 0;
            speed = s;
            color = c;
        }

    }
}
