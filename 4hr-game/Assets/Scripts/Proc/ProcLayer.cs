using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NineFive.Proc
{
    [System.Serializable]
    abstract public class ProcLayer
    {
        public abstract void Apply(Store store, System.Random random);
    }
}
