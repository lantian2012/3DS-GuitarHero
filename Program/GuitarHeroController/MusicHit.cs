using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarHeroController
{
    class MusicHit
    {
        public int Speed;
        private int _allNum;
        public int[] Time;
        public int[] Position;
        MusicHit(int all) {
            _allNum = all;
            Time = new int[_allNum];
            Position = new int[_allNum];
        }
                

    }
}
