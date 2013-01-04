using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarHeroController
{
    public class CString
    {
        public List<Note> notes;
        public string color;
        private const int hitzoneTop = 560;
        private const int hitzoneBottom = 660;

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
                if (note.isScorable && note.Y >= hitzoneTop && note.Y <= hitzoneBottom && note.isAlive)
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
            List<Note> listtodelete = new List<Note>();
            foreach (Note note in notes)
            {
                note.Y += (768 / note.speed / frameRate);
                if (note.Y > 768)
                    listtodelete.Add(note);
                if (!note.isAlive)
                {
                    if ((double)note.Opacity - 2 / (double)frameRate > 0)
                        note.Opacity = (double)note.Opacity - 2 / (double)frameRate;
                    note.Height += frameRate;
                }
            }
            foreach (Note note in listtodelete)
                notes.Remove(note);
        }
        /// <summary>
        /// remove the note when it is hitted
        /// </summary>
        public void hit()
        {
            foreach (Note note in notes)
            {
                if (note.Y >= hitzoneTop && note.Y <= hitzoneBottom)
                    note.isAlive = false;
            }
        }
        /// <summary>
        /// When hit at wrong time, the next note is turned unscorable
        /// </summary>
        public void updateScorability()
        {
            int i = 0;
            while (i < notes.Count)
            {
                if (notes[i].Y >= hitzoneTop - 100 && notes[i].Y <= hitzoneTop)
                    notes[i].isScorable = false;
                break;
            }
        }
    }
}
