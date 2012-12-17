using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarHeroController
{
    class CString
    {
        public List<Note> notes;
        public string color;

        public CString()
        {
            notes = new List<Note>();
        }
        /// <summary>
        /// check if the string has one note in the hitting area
        /// </summary>
        /// <returns>
        /// true iff a note is in the hitting area
        /// </returns>
        public bool isReadyForHit()
        {
            bool isReady = false;
            foreach (Note note in notes)
            {
                if (note.Y > 650 && note.Y < 750)
                    isReady = true;
            }
            return isReady;
        }
        /// <summary>
        /// update positions of all notes on current string
        /// </summary>
        /// <param name="frameRate"></param>
        public void advanceFrame(int frameRate)
        {
            foreach (Note note in notes)
            {
                note.Y += (768 / note.speed / frameRate);
                note.Width += (125 / note.speed / frameRate);
                note.X = (250 - note.Width) / 2;
            }
        }
    }
}
