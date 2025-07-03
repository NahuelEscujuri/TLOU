using UnityEngine;
using System.Collections;
using System;

namespace Automata
{
    public class Detectable : MonoBehaviour, IDetectable
    {
        public event Action<float> OnValueChanged;
        [SerializeField] NewMovementController movementController;
        float value;
        Coroutine updateValue;
        public float GetValue() => value;

        #region UpdateValue Logic
        private void OnEnable() => updateValue = StartCoroutine(UpdateValue(0.3f));
        private void OnDisable() => StopCoroutine(updateValue);
        IEnumerator UpdateValue(float delay)
        {
            var wait = new WaitForSeconds(delay);

            while (true)
            {
                yield return wait;

                var newValue = movementController.IsRun() ? 1f : movementController.IsCrouch() ? .5f : .8f; 
                if(newValue != value)
                {
                    value = newValue;
                    OnValueChanged?.Invoke(value);
                }
            }
        }
        #endregion
    }
}