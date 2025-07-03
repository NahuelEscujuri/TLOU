using System.Linq;
using UnityEngine;

namespace HeartRateSystem
{
    public class RunHeartRateState : IHeartRateState
    {
        readonly IMovement movement;
        readonly HeartRateController heartRateController;
        readonly RateSoundsLibrary rateSoundsLibrary;

        public RunHeartRateState(IMovement movement, HeartRateController heartRateController, RateSoundsLibrary rateSoundsLibrary)
        {
            this.movement = movement;
            this.heartRateController = heartRateController;
            this.rateSoundsLibrary = rateSoundsLibrary;
        }

        public virtual float GetTargetHeartRate() => IsRunning() ? 1f : 0f;
        bool IsRunning() => movement.IsMoving() && movement.IsRun();
        public virtual AudioClip GetBreathingSound() => rateSoundsLibrary.GetAudioClipForHeartRate(heartRateController.HeartRate);
    }
}

