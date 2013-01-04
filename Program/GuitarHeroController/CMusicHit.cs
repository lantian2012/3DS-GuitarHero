using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace GuitarHeroController
{
    /// <summary>
    /// pack the data from MusicHit files
    /// Requirement for MusicHit file should be as follows:
    /// The first line is a integer, defining its length;
    /// The second line is a integer, defining its overall speed;
    /// Then there are 2*length lines
    /// The line with time data should be like:
    /// t:xxx, xxx is an integer which defines the time
    /// The line with position data should be like:
    /// p:xxx, xxx is an integer whichdefines the position
    /// </summary>
    public class CMusicHit
    {
        #region Attributes
        private int _length;
        private int _speed;
        private int[] _time;
        private int[] _position;
        private int _next;
        private  int _endTime;

        public int Speed {
            get { return this._speed; }
        }
        public int EndTime {
            get { return this._endTime; }
        }
        #endregion
        public CMusicHit(String path) {
            int counter = 0;
            this._next = 0;
            string line;
            StreamReader hitFile = new StreamReader(path);

            line = hitFile.ReadLine();
            this._length = int.Parse(line);
            line = hitFile.ReadLine();
            this._speed = int.Parse(line);
            line = hitFile.ReadLine();
            this._endTime = int.Parse(line);

            this._time = new int[this._length];
            this._position = new int[this._length];
            while ((line = hitFile.ReadLine()) != null) {
                if (line.Contains('t'))
                {
                    line = line.Substring(line.IndexOf(':')+1);
                    this._time[counter] = int.Parse(line);
                }
                else {
                    line = line.Substring(line.IndexOf(':')+1);
                    this._position[counter] = int.Parse(line);
                    counter++;
                }
            }
        }
        #region Next
        public int NextTime {
            get
            {
                if (this._length <= this._next)
                {
                    return -1;
                }
                else
                {
                    return this._time[this._next];
                }
            }
        }
        public int NextPosition {
            get {
                if (this._length <= this._next)
                {
                    return -2;
                }
                else {
                    var pos = this._position[this._next];
                    this._next++;
                    return pos;
                }
            }
        }
        #endregion
    }
}
