using System;
using UnityEngine;

namespace Automata
{
    public class CoverPoint: MonoBehaviour
    {
        public static event Action<CoverPoint> OnEnableCover;
        public static event Action<CoverPoint> OnDisableCover;
        private void OnEnable() => OnEnableCover?.Invoke(this);
        private void OnDisable() => OnDisableCover?.Invoke(this);

        public Transform pointTransform;
        public bool isOcupped;
    }
}