using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarHeroController
{
    public class CNote
    {
        #region Attributes
        private List<CString> Strings;
        private int FrameRate;
        public int Speed { get; set; }
        #endregion

        /// <summary>
        /// initilize CNote
        /// </summary>
        /// <param name="numberOfStrings"></param>
        public CNote(int numberOfStrings)
        {
            string[] colors = new string[] { "Aquamarine", "Crimson", "DarkOliveGreen", "Gray" };
            FrameRate = 30;
            Speed = 10;
            Strings = new List<CString>();
            for (int i = 0; i < numberOfStrings; i++)
            {
                Strings.Add(new CString());
                Strings[i].color = colors[i];
            }
        }

        /// <summary>
        /// update positions of all notes
        /// </summary>
        public void advanceFrame()
        {
            foreach (CString stringitem in Strings)
                stringitem.advanceFrame(FrameRate);
        }
        /// <summary>
        /// add a new note in data
        /// </summary>
        /// <param name="stringNumber"></param>
        /// <returns></returns>
        public Note addNewNote(int stringNumber)
        {
            Strings[stringNumber].notes.Add(new Note(Speed, Strings[stringNumber].color));
            return Strings[stringNumber].notes[Strings[stringNumber].notes.Count - 1];
        }
        /// <summary>
        /// detect if a certain string is ready for hit
        /// </summary>
        /// <param name="StringNumber"></param>
        /// <returns></returns>
        public bool isStringReadyForHit(int StringNumber)
        {
            return Strings[StringNumber].isReadyForHit();
        }
        /// <summary>
        /// deal with mess after hitting a Note
        /// </summary>
        /// <param name="StringNumber"></param>
        public void hit(int StringNumber)
        {
            Strings[StringNumber].hit();
        }

        public void updateScorability(int StringNumber)
        {
            Strings[StringNumber].updateScorability();
        }
        public void setFrameRate(int frameRate)
        {
            FrameRate = frameRate;
        }

    }
}
