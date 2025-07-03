using System.Collections;
using UnityEngine;

namespace HeartRateSystem
{
    public interface IHeartRateState
    {
        float GetTargetHeartRate();
        AudioClip GetBreathingSound();
    }
}

