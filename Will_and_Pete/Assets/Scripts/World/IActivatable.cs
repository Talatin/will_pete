using System;
using UnityEngine;

namespace World
{
    public abstract class AActivatable : MonoBehaviour
    {
        public abstract void Activate();
        public abstract void Deactivate();
    }
}