using System;
using UnityEngine;

namespace AR.Core
{
    public class GameObjectArgs : EventArgs
    {
        public readonly GameObject GO;

        public GameObjectArgs(GameObject GO)
        {
            this.GO = GO;
        }
    }
}

