using System;
using UnityEngine;
using UnityEngine.UI;

namespace Automata
{
    public class UIStateIconHandler: MonoBehaviour
    {
        [Header("Alarm")]
        [SerializeField] Sprite alarmIcon;
        [SerializeField] float alarmDuration = .75f;
        [Header("States")]
        [SerializeField] Sprite[] icons;
        [Header("References")]
        [SerializeField] PDAController controller;
        [SerializeField] Image stateImage;
        AlarmEmiter alarmEmiter;
        Sprite currentStateSprite;

        private void Awake() => alarmEmiter = controller.GetComponent<AlarmEmiter>();
        private void OnEnable()
        {
            alarmEmiter.OnAlarming += ResolveAlarm;
            controller.OnCurrentStateChanged += ResolveStateChanged;
        }
        private void OnDisable()
        {
            alarmEmiter.OnAlarming -= ResolveAlarm;
            controller.OnCurrentStateChanged -= ResolveStateChanged;
        }
        void ResolveAlarm()
        {
            stateImage.sprite = alarmIcon;
            Invoke(nameof(SetStateCurrent), alarmDuration);
        }
        void SetStateCurrent() => stateImage.sprite = currentStateSprite;
        void ResolveStateChanged(State state)
        {
            currentStateSprite = state switch
            {
                IdleState => icons[0],
                EnemyPatrolState => icons[1],
                EnemyPanicState => icons[2],
                EnemySearchState => icons[3],
                EnemyShootState => icons[4],
                _ => null,
            };
            SetStateCurrent();
        }
    } 
}