using System;
using UnityEngine;

namespace HeartRateSystem
{
    [Serializable]
    public struct RateSounds
    {
        [Range(0f,1f)]public float rate;
        public AudioClip[] sounds;
    }
}

