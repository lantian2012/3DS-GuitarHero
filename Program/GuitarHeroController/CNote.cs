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
        private List<CString> strings;
        private int frameRate;
        #endregion

        /// <summary>
        /// initilize CNote
        /// </summary>
        /// <param name="numberOfStrings"></param>
        public CNote(int numberOfStrings)
        {
            string[] colors = new string[] { "Aquamarine", "Crimson", "DarkOliveGreen", "Gray" };
            frameRate = 1;
            strings = new List<CString>();
            for (int i = 0; i < numberOfStrings; i++)
            {
                strings.Add(new CString());
                strings[i].color = colors[i];
            }
        }

        /// <summary>
        /// update positions of all notes
        /// </summary>
        public void advanceFrame()
        {
            foreach (CString stringitem in strings)
                stringitem.advanceFrame(frameRate);
        }

        public Note addNewNote(int stringNumber, int speed)
        {
            strings[stringNumber].notes.Add(new Note(speed, strings[stringNumber].color));
            return strings[stringNumber].notes[strings[stringNumber].notes.Count - 1];
        }
    }
}
