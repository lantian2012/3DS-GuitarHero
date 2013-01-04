using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GuitarHeroModel
{
    /// <summary>
    /// The OptionFile should be like:
    /// </summary>
    public class MOptions
    {
        public string MusicMediaFilePath;
        public string MusicHitFilePath;
        public int Difficulty;
        public int NumberOfMusic;
        private List<string> _musicName;
        public int LastSelected;
        public MOptions() {
        }
        public void loadOptionFile() {
            string line = System.IO.Directory.GetCurrentDirectory();
            line += "\\Option\\OptionFile.3dsopt";
            StreamReader optionFile = new StreamReader(line);

            line = optionFile.ReadLine();
            line = line.Substring(line.IndexOf(':') + 1);
            this.Difficulty = int.Parse(line);

            line = optionFile.ReadLine();
            line = line.Substring(line.IndexOf(':') + 1);
            this.NumberOfMusic = int.Parse(line);
            this._musicName = new List<string>();

            for (int i = 0; i < this.NumberOfMusic; i++) {
                line = optionFile.ReadLine();
                line = line.Substring(line.IndexOf(':') + 1);
                this._musicName.Add(line);
            }

            line = optionFile.ReadLine();
            line = line.Substring(line.IndexOf(':') + 1);
            this.LastSelected = int.Parse(line);

            optionFile.Close();
            getPathOfMusic();
        }
        /// <summary>
        /// Save the optionFile
        /// Do NOT try to modify music number and names with this function
        /// </summary>
        /// <param name="diff">Difficulty from OptionPage</param>
        /// <param name="last">LastChosenMusicIndex from OptionPage</param>
        public void saveOptionFile(int diff, int last) {
            string line = System.IO.Directory.GetCurrentDirectory();
            line += "\\Option\\OptionFile.3dsopt";
            StreamWriter optionFile = new StreamWriter(line);

            line = "Difficulty:" + diff.ToString();
            optionFile.WriteLine(line);

            line = "NumberOfMusic:" + this.NumberOfMusic.ToString();
            optionFile.WriteLine(line);

            for (int i = 0; i < this.NumberOfMusic; i++) {
                line = "Name:" + this._musicName.ElementAt(i);
                optionFile.WriteLine(line);
            }

            line = "Last:" + last.ToString();
            optionFile.WriteLine(line);

            optionFile.Close();

        }
        private void getPathOfMusic()
        {
            string line = System.IO.Directory.GetCurrentDirectory();
            line += "\\Music\\" + this._musicName.ElementAt(this.LastSelected);
            string path = line + ".wav";
            this.MusicMediaFilePath = path;
            switch (this.Difficulty) {
                case 0: {
                    path = line + "_easy.3dsmh";
                    break;
                }
                case 1: {
                    path = line + "_normal.3dsmh";
                    break;
                }
                case 2: {
                    path = line + "_hard.3dsmh";
                    break;
                }
                default: {
                    path = line + "_normal.3dsmh";
                    break;
                }
            }
            this.MusicHitFilePath = path;
        }
        public string getMusicName(int index) {
            return this._musicName.ElementAt(index);
        }
    }
}
