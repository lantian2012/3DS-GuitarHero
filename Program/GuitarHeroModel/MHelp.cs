using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GuitarHeroModel
{
    class MHelp
    {
        #region attribute
        public string[] _help;
        StreamReader _loadHelpFile;
        string _helpFilePath;
        #endregion

        #region operation
        /// <summary>
        /// load help info from helpFile
        /// </summary>
        public void loadHelpFile()
        {
 //         this._help = new string[200];
            this._helpFilePath = System.IO.Directory.GetCurrentDirectory();
            this._helpFilePath += "\\Help\\HelpFile.txt";
            _loadHelpFile = new StreamReader(this._helpFilePath);
            string _content = _loadHelpFile.ReadToEnd();
            this._help = _content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        }
        #endregion
    }
}
