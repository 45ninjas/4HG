using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NineFive.Proc
{
    abstract public class ProcLayer : ScriptableObject
    {
        public abstract void Apply(Store store, System.Random random);
    }
}