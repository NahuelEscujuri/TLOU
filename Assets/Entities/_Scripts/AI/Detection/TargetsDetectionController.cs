using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Automata
{
    public class TargetsDetectionController : MonoBehaviour
    {
        public event Action OnTargetsChanged;
        public event Action OnTargetsInfoUpdated;
        public Dictionary<Transform, TargetInfo> TargetsDetected => targetsDetected;
        [Header("References")]
        IFaction faction;
        [Header("Targets")]
        Dictionary<Transform, TargetInfo> targetsDetected = new();
        Dictionary<Transform, TargetInfo> targetsInView = new();
        [SerializeField] FieldOfView fieldOfView;
        bool wasDetected;
        Coroutine checkCor;

        public class TargetInfo
        {
            public IDetectable detection;
            public IFaction faction;
            public float sympathy;

            public TargetInfo(IDetectable detection, IFaction faction, float sympathy)
            {
                this.detection = detection;
                this.faction = faction;
                this.sympathy = sympathy;
            }
        }
        private void Awake() => faction = GetComponent<IFaction>();
        void OnEnable()
        {
            checkCor = StartCoroutine(CheckDetectionWithDelay(.2f));
            fieldOfView.OnTargetDetected += HandleEntityEnteredFieldOfView;
            fieldOfView.OnTargetLost += HandleEntityLost;
        }
        void OnDisable()
        {
            StopCoroutine(checkCor);
            fieldOfView.OnTargetDetected -= HandleEntityEnteredFieldOfView;
            fieldOfView.OnTargetLost -= HandleEntityLost;
        }

        #region check detection
        IEnumerator CheckDetectionWithDelay(float delay)
        {
            var wait = new WaitForSeconds(delay);
            while (true)
            {
                yield return wait;
                CheckDetection();
            }
        }
        void CheckDetection()
        {
            bool hasChanged = false;
            foreach (var tiv in targetsInView)
            {
                if (tiv.Value.detection.GetValue() <= 0)
                {
                    if (targetsDetected.ContainsKey(tiv.Key))
                    {
                        targetsDetected.Remove(tiv.Key);
                        hasChanged = true;
                    }
                    continue;
                }
                if (!targetsDetected.ContainsKey(tiv.Key))
                {
                    targetsDetected[tiv.Key] = tiv.Value;
                    hasChanged = true;
                }
                targetsDetected[tiv.Key].sympathy = GetSympathy(tiv.Value.faction);
            }
            if (hasChanged)
                OnTargetsChanged?.Invoke();
            OnTargetsInfoUpdated?.Invoke();
        }
        #endregion

        #region handle enter/lost
        void HandleEntityEnteredFieldOfView(Transform target)
        {
            if (!target.TryGetComponent(out IDetectable detectionValue)) return;
            var targetFaction = target.GetComponent<IFaction>();

            targetsInView[target] = new TargetInfo(detectionValue, targetFaction, GetSympathy(targetFaction));
        }
        void HandleEntityLost(Transform target)
        {
            if (!target.TryGetComponent(out IDetectable detectionValue)) return;

            targetsInView.Remove(target);
            if (targetsDetected.ContainsKey(target))
            {
                targetsDetected.Remove(target);
                OnTargetsChanged?.Invoke();
            }
        }
        #endregion
        float GetSympathy(IFaction otherFaction) => (otherFaction != null) ? this.faction.ObtainSympathy(otherFaction) : 0f;
    }
}