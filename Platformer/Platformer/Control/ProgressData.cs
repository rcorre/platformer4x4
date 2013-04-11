using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Platformer.Model;

namespace Platformer.Control
{
    public class ProgressData
    {
        public int NumCoins;
        public int CurrentLevel;
        public bool[] LevelCompleted;
        Weapon CurrentWeapon;
    }
}
