using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Media;
using System.Windows;
using GuitarHeroModel;

namespace GuitarHeroController
{

    public class CLogic : INotifyPropertyChanged
    {

        #region Attributes
        private const int hitzoneTop = 560;
        private const int hitzoneBottom = 660;
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private CNote _CNote;
        public CShadow LeftShadow;
        public CShadow RightShadow;
        private CMusicHit _CMusicHit;
        private int _TotalTime;
        public bool isStart { get; set; }
        public bool isConstantHit { get; set;}
        private DateTime StartTime;
        private TimeSpan _TimeSpan;
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
        private SoundPlayer _SoundPlayer;
        #endregion

        #region Private Varibles Used
        private bool[] isStringHit;
        private SoundPlayer _player;
        #endregion

        public CLogic(MOptions option)
        {
            
            _SoundPlayer = new SoundPlayer();
            _CNote = new CNote(3);
            LeftShadow = new CShadow(250,150);
            RightShadow = new CShadow(450,150);
            _CMusicHit = new CMusicHit(option.MusicHitFilePath);
            _SoundPlayer.SoundLocation = @option.MusicMediaFilePath;
            //_SoundPlayer.SoundLocation = @"D:\Gangnam.wav";
            _SoundPlayer.LoadAsync();
            _CNote.Speed = _CMusicHit.Speed;
            _TotalTime = _CMusicHit.EndTime;
            Score = 0;
            Progress = 0;
            isStart = false;
        }

        /// <summary>
        /// start timer, play music, and load Notes
        /// </summary>
        public void startGame()
        {
            isStart = true;
            
            if (_SoundPlayer.IsLoadCompleted)
            {
                try
                {
                    _SoundPlayer.Play();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, "Error playing sound");
                }
            }
            _TimeSpan = new TimeSpan();
            StartTime = DateTime.Now;
        }


        /// <summary>
        /// check string and update when hit
        /// </summary>
        /// <remarks>
        /// Consist 3 Parts:
        /// 1. check which strings are hit
        /// 2. check if the hit string is ready for hit
        /// 3. update string and isConstantHit
        /// </remarks>
        public void checkString()
        {
            isShadowOnString();
            for (int i = 0; i < 3; i++)
            {
                if (isStringHit[i])
                    if (_CNote.isStringReadyForHit(i))
                    {
                        Score++;
                        _CNote.hit(i);
                    }
                    else
                    {
                        isConstantHit = true;
                        _CNote.updateScorability(i);
                    }
            }
        }

        private void isShadowOnString()
        {
            isStringHit = new bool[] {false, false, false};
            if (LeftShadow.Y <= hitzoneBottom && LeftShadow.Y >= hitzoneTop)
            {
                if (LeftShadow.X >= 0 && LeftShadow.X <= 270)
                    isStringHit[0] = true;
                else if (LeftShadow.X > 270 && LeftShadow.X <= 540)
                    isStringHit[1] = true;
                else if (LeftShadow.X > 540 && LeftShadow.X <= 810)
                    isStringHit[2] = true;
            } 
            if (RightShadow.Y <= hitzoneBottom && RightShadow.Y >= hitzoneTop)
            {
                if (RightShadow.X >= 0 && RightShadow.X <= 270)
                    isStringHit[0] = true;
                else if (RightShadow.X > 270 && RightShadow.X <= 540)
                    isStringHit[1] = true;
                else if (RightShadow.X > 540 && RightShadow.X <= 810)
                    isStringHit[2] = true;
            }
            if (!isStringHit[0] && !isStringHit[1] && !isStringHit[2])
            {
                isConstantHit = false;
            }
        }
        /// <summary>
        /// Check if there is new note ready to be added.
        /// </summary>
        /// <remarks>
        /// Return value:
        /// 0 or a positive integer: represents the index of the string
        /// -1: there is currently no new note
        /// -2:the music is over
        /// Notice that -2 does NOT necessarily result in the gameOver logic
        /// as there may be a part of music left to be played
        /// </remarks>
        /// <returns></returns>
        public int checkNewNote()
        {
            
            int timeInterval = getTimeInterval();
            if (this._CMusicHit.NextTime > 0)
            {
                if (this._CMusicHit.NextTime <= timeInterval)
                {
                    return this._CMusicHit.NextPosition;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -2;
            }
        }

        public Note addNewNote(int StringNumber)
        {
            return _CNote.addNewNote(StringNumber);
        }

        /// <summary>
        /// get the time interval between when the music began and now
        /// </summary>
        /// <remarks>
        /// Unit: Millisecond
        /// update Progress
        /// </remarks>
        /// <returns></returns>
        private int getTimeInterval()
        {
            int interval = 0;
            _TimeSpan = DateTime.Now - StartTime;
            interval = (int)_TimeSpan.TotalMilliseconds;
            if (interval < _TotalTime)
                Progress = interval * 100 / _TotalTime;
            else
                Progress = 100;
            return interval;
        }
        /// <summary>
        /// update the location of Notes
        /// </summary>
        public void advanceFrame()
        {
            _CNote.advanceFrame();
        }

        public void setFrameRate(int frameRate)
        {
            _CNote.setFrameRate(frameRate);
        }
        /// <summary>
        /// check if the music ends
        /// </summary>
        /// <returns>
        /// true: when music ends
        /// false: music playing
        /// </returns>
        public bool checkGameOver()
        {
            _TimeSpan = DateTime.Now - StartTime;
            if ((int)_TimeSpan.TotalMilliseconds > _TotalTime)
            {
                return true;
            }
            else
                return false;
        }
        
    }
}
