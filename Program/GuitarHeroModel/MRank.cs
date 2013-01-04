using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GuitarHeroModel
{
    public class MRank
    {
        #region Attributes
        string _rankFilePath;
        public string[] _name;
        public int[] _rank;
        StreamReader _loadRankFile;
        StreamWriter _updateRankFile;
        #endregion

        #region operation
        /// <summary>
        /// load the rank info from rankFile
        /// </summary>
        public void loadRankFile()
        {
            this._rankFilePath = System.IO.Directory.GetCurrentDirectory();
            this._rankFilePath += "\\Rank\\RankFile.3dsrf";
            this._loadRankFile = new StreamReader(this._rankFilePath);
            string _content = _loadRankFile.ReadToEnd();
            string[] _str = _content.Split( new string[]{ "\r\n" },StringSplitOptions.None);
            this._name = new string[ 5 ];
            this._rank = new int[ 5 ];
            for( int i = 0; i < 10; i++ )
            {
                if( i % 2 == 0 )
                {
                    this._name[i>>1] = _str[i];
                }
                else
                {
                    this._rank[i>>1] = System.Int32.Parse(_str[i]);
                }
            }
            this._loadRankFile.Close();
        }
        /// <summary>
        /// update the rank info in the rankFile
        /// </summary>
        public void updateRankFile()
        {
            this._rankFilePath = System.IO.Directory.GetCurrentDirectory();
            this._rankFilePath += "\\Rank\\RankFile.3dsrf";
            string[] _str = new string [2*this._rank.Length];
            int j = 0;
            for (int i = 0; i < this._name.Length; i++)
            {
                _str[j] = this._name[this._name.Length - i - 1];
                j++;
                _str[j] = this._rank[this._name.Length - i - 1].ToString();
                j++;
            }
            using (this._updateRankFile = new StreamWriter(this._rankFilePath))
            {
                for (int i = 0; i < _str.Length; i++)
                {
                    _updateRankFile.WriteLine(_str[i]);
                }
            }
        }
        #endregion
    }
}
