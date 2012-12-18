using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarHeroController
{
    class CNote
    {
        #region Attributes
        private List<CString> Strings;
        private int FrameRate;
        private int Speed;
        #endregion

        /// <summary>
        /// initilize CNote
        /// </summary>
        /// <param name="numberOfStrings"></param>
        public CNote(int numberOfStrings)
        {
            string[] colors = new string[] { "Aquamarine", "Crimson", "DarkOliveGreen", "Gray" };
            FrameRate = 1;
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

        public Note addNewNote(int stringNumber)
        {
            Strings[stringNumber].notes.Add(new Note(Speed, Strings[stringNumber].color));
            return Strings[stringNumber].notes[Strings[stringNumber].notes.Count - 1];
        }
    }
}
