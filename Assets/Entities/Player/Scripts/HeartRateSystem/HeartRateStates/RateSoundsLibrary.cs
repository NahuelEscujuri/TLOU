using Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HeartRateSystem
{
    public class RateSoundsLibrary
    {
        public RateSounds[] ratesSounds;
        public RateSoundsLibrary(IEnumerable<RateSounds> rateSounds)
        {
            this.ratesSounds = rateSounds.OrderByDescending(rs => rs.rate).ToArray();
        }
        public AudioClip GetAudioClipForHeartRate(float heartRate) => GetRateSoundsForHeartRate(heartRate).sounds.GetRandomElement();
        RateSounds GetRateSoundsForHeartRate(float heartRate) => ratesSounds.First(rs => rs.rate <= heartRate);
    }
}

