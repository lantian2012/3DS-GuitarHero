using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuitarHeroModel;

namespace GuitarHeroController
{
    public class CRank
    {
        #region attribute
        MRank _mRank;
        #endregion
        
        #region operation

        public CRank()
        {
            this._mRank = new MRank();
            _mRank.loadRankFile();
        }

        /// <summary>
        /// update the rank info when game over
        /// </summary>
        /// <param name="name">
        /// the player whoes score should be put into the rank table
        /// </param>
        /// <param name="rank">
        /// its score
        /// </param>
        public void updateRank(string name, int rank)
        {
            Array.Sort(this._mRank._rank, this._mRank._name);
            this._mRank._rank[0] = rank;
            this._mRank._name[0] = name;
            Array.Sort(this._mRank._rank,this._mRank._name);
            _mRank.updateRankFile();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// return the rank info in order
        /// </returns>
        public string[] loadName()
        {
            _mRank.loadRankFile();
            return this._mRank._name;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// return the name info in the order of according rank
        /// </returns>
        public int[] loadRank()
        {
            _mRank.loadRankFile();
            return this._mRank._rank;
        }
        public int getLowest()
        {
            return this._mRank._rank[0];
        }
        #endregion
    }
}
