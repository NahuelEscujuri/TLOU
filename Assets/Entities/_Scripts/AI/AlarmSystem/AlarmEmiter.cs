using System;
using UnityEngine;

namespace Automata
{
    /// <summary>
    /// Posee un metodo publico que al llamarlo lanza el AlarmData a Recibidores que se encuentren en el rango
    /// </summary>
    public class AlarmEmiter : MonoBehaviour
    {
        public event Action OnAlarming;

        [SerializeField] LayerMask layerMask;
        [SerializeField] float alarmRange;

        public void EmitAlarm(AlarmData alarmData)
        {
            OnAlarming?.Invoke();
            foreach (var collider in Physics.OverlapSphere(transform.position, alarmRange, layerMask))
            {
                if (!collider.TryGetComponent<AlarmReceiver>(out var alarmReceiver)) continue;

                alarmReceiver.Receive(alarmData);
            }
        }
    }
}