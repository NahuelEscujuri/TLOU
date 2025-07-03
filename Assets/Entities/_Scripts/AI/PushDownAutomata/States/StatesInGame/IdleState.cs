using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Automata
{
    public class IdleState : State
    {
        Animator animator;
        TargetsDetectionController detection;
        AlarmReceiver alarmReceiver;
        bool hasToEnd;
        static readonly int LeaveIdleHash = Animator.StringToHash("LeaveIdle");
        static readonly int EnterIdleHash = Animator.StringToHash("EnterIdle");
        public IdleState(PushDownAutomata pda) : base(pda){}
        public override void Enter()
        {
            hasToEnd = false;
            detection = pda.Context.targetDetection;
            animator = pda.Context.movement.GetComponent<Animator>();
            alarmReceiver = pda.Context.alarmReceiver;
        }
        public override void Enable()
        {
            animator.Play(EnterIdleHash);
            animator.SetBool(LeaveIdleHash, false);
            detection.OnTargetsChanged += CheckTargets;
            alarmReceiver.OnAlarmReceived += CheckAlarm;
        }
        public override void Update() { }
        public override void Disable()
        {
            detection.OnTargetsChanged -= CheckTargets;
            alarmReceiver.OnAlarmReceived -= CheckAlarm;
            animator.SetBool(LeaveIdleHash, true);
        }
        public override void Exit() { }
        void CheckAlarm(AlarmData alarmData)
        {
            if(alarmData.state == AlarmData.State.Found)
            {
                pda.PushState(new EnemyShootState(pda, alarmData.targetTransform, 10f, 3f, 5f));
                return;
            }
            if (alarmData.state == AlarmData.State.Searching)
            {
                pda.PushState(new EnemySearchState(pda, 5f, 3f, alarmData.targetTransform));
                return;
            }
        }
        void CheckTargets()
        {
            if(hasToEnd) return;

            KeyValuePair<Transform, TargetsDetectionController.TargetInfo>? selectedTarget = null; 
            foreach (var targets in detection.TargetsDetected)
            {
                if (targets.Value.sympathy >= 0) continue;

                selectedTarget = targets;
            }
            if (!selectedTarget.HasValue) return;
            
            hasToEnd = true;
        }
    }
}