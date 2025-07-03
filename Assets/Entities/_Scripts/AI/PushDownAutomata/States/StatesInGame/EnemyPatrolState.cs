using System.Collections.Generic;
using UnityEngine;

namespace Automata
{
    public class EnemyPatrolState : State
    {
        TargetsDetectionController detection;
        IAMovementController movement;
        int currentPatrolIndex;
        AlarmReceiver alarmReceiver;
        public EnemyPatrolState(PushDownAutomata pda) : base(pda) { }
        public override void Enter()
        {
            detection = pda.Context.targetDetection;
            movement = pda.Context.movement;
            alarmReceiver = pda.Context.alarmReceiver;
        }
        public override void Enable()
        {
            alarmReceiver.OnAlarmReceived += CheckAlarm;
            detection.OnTargetsChanged += CheckTargets;
            movement.SetNormalSpeed();
            movement.SetPositionToMove(pda.Context.patrolPoints[currentPatrolIndex].position);
        }
        public override void Update() => Patrol();
        void Patrol()
        {
            if (movement.HasReachedDestination())
            {
                if (pda.Context.patrolPoints.Length == 1) // si solo va a quedar en un lugar
                {
                    pda.PushState(new IdleState(pda)); // seteamos el estado idle especial
                    return;
                }

                currentPatrolIndex = (currentPatrolIndex + 1) % pda.Context.patrolPoints.Length;
                movement.SetPositionToMove(pda.Context.patrolPoints[currentPatrolIndex].position);
            }
        }
        public override void Disable()
        {
            detection.OnTargetsChanged -= CheckTargets;
            alarmReceiver.OnAlarmReceived -= CheckAlarm;
        }
        public override void Exit() { }
        void CheckAlarm(AlarmData alarmData)
        {
            if (alarmData.state == AlarmData.State.Found)
            {
                pda.PushState(new EnemyShootState(pda, alarmData.targetTransform, 10f, 3f, 5f));
                return;
            }
            if (alarmData.state == AlarmData.State.Searching)
            {
                pda.PushState(new EnemySearchState(pda, 50f, 3f, alarmData.targetTransform));
                return;
            }
        }
        void CheckTargets()
        {
            KeyValuePair<Transform, TargetsDetectionController.TargetInfo>? selectedTarget = null;
            foreach (var targets in detection.TargetsDetected)
            {
                if (targets.Value.sympathy >= 0) continue;

                selectedTarget = targets;
            }
            if (!selectedTarget.HasValue) return;

            pda.PushState(new EnemyPanicState(pda, selectedTarget.Value.Key, 2f));
        }
    }
}