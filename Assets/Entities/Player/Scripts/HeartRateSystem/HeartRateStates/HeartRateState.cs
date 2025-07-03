using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HeartRateSystem
{
    public class HeartRateState : IHeartRateState
    {
        readonly HeartRateController heartRateController;
        readonly RateSoundsLibrary rateSoundsLibrary;
        protected readonly float heartRate;
        public HeartRateState(HeartRateController heartRateController,float heartRate, RateSoundsLibrary rateSoundsLibrary)
        {
            this.heartRate = heartRate;
            this.rateSoundsLibrary = rateSoundsLibrary;
            this.heartRateController = heartRateController;
        }
        public virtual float GetTargetHeartRate() => heartRate;
        public virtual AudioClip GetBreathingSound() => rateSoundsLibrary.GetAudioClipForHeartRate(heartRateController.HeartRate);
    }
}

