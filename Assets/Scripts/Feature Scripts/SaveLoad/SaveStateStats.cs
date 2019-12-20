using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class SaveStateStats
    {
        public int Money;
        public Vector2 Position;
        public int[] BlockAmounts;
        public int Seed;
    }
}
