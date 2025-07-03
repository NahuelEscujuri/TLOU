using UnityEngine;
using System.Collections;
using System;

namespace Automata
{
    /// <summary>
    /// Recibe AlarmData y setea la última como la alarma actual limpiandola pasada una duración
    /// </summary>
    public class AlarmReceiver : MonoBehaviour
    {
        public event Action<AlarmData> OnAlarmReceived;
        [SerializeField] float alarmDataDuration = 2f;
        public AlarmData? currentAlarmData;
        Coroutine removeAlarmCoroutine;
        public void Receive(AlarmData alarmData)
        {
            currentAlarmData = alarmData;
            
            OnAlarmReceived?.Invoke(alarmData);
            if(removeAlarmCoroutine != null) StopCoroutine(removeAlarmCoroutine);                
            removeAlarmCoroutine = StartCoroutine(RemoveAlarm());
        }
        IEnumerator RemoveAlarm()
        {
            yield return new WaitForSeconds(alarmDataDuration);
            currentAlarmData = null;            
        }
    }
}