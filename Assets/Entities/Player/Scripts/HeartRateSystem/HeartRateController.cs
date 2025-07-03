using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HeartRateSystem
{
    public class HeartRateController : MonoBehaviour
    {
        [SerializeField] float lerpSpeed = 0.1f;
        [SerializeField, Range(0f, 1f)] float heartRate;
        public float HeartRate => heartRate;
        [SerializeField] AudioSource audioSource;
        [Header("Dafult")]
        [SerializeField] RateSounds[] defaultClips;
        IHeartRateState defaultState;
        IHeartRateState currentState;
        List<IHeartRateState> activeStates = new();

        public void AddState(IHeartRateState stateToAdd)
        {
            if (activeStates.Contains(stateToAdd)) return;

            activeStates.Add(stateToAdd);
            UpdateCurrentState();
        }
        public void RemoveState(IHeartRateState stateToRemove)
        {
            if (!activeStates.Contains(stateToRemove)) return;

            activeStates.Remove(stateToRemove);
            UpdateCurrentState();
        }
        void UpdateCurrentState()
        {
            var possibleNewState = activeStates.OrderByDescending(s => s.GetTargetHeartRate()).First();
            currentState = possibleNewState;
        }

        #region unity lifecycle
        private void Awake()
        {
            defaultState = new HeartRateState(this, 0.01f, new RateSoundsLibrary(defaultClips));
            AddState(defaultState);
        }
        private IEnumerator Start()
        {
            var wait = new WaitForSeconds(.1f);
            while (true)
            {
                yield return wait;  
                UpdateCurrentState();
            }
        }
        void Update()
        {
            if(currentState == null) return;
            UpdateHeartRate();
            UpdateSound();
        }
        void UpdateHeartRate() => heartRate = Mathf.Lerp(heartRate, currentState.GetTargetHeartRate(), Time.deltaTime * lerpSpeed);
        void UpdateSound()
        { 
            if(audioSource.isPlaying) return;

            audioSource.clip = currentState.GetBreathingSound();
            audioSource.Play();
        }
        #endregion
    }
}

