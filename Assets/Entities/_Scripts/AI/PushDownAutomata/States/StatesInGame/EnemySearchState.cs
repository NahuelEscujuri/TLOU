using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Automata
{
    public class EnemySearchState : State
    {
        readonly Transform targetTransform;
        readonly float maxDistance;
        float timeToSearch;
        TargetsDetectionController detection;
        IAMovementController movement;
        AlarmReceiver alarmReceiver;
        Vector3? randomPosition;
        public EnemySearchState(PushDownAutomata pda, float timeToSearch, float maxDistance,Transform targetTransform) : base(pda) 
        { 
            this.timeToSearch = timeToSearch;
            this.maxDistance = maxDistance;
            this.targetTransform = targetTransform;
        }
        public override void Enter()
        {
            detection = pda.Context.targetDetection;
            movement = pda.Context.movement;
            alarmReceiver = pda.Context.alarmReceiver;
        }
        public override void Enable()
        {
            detection.OnTargetsChanged += CheckTargets;
            alarmReceiver.OnAlarmReceived += CheckAlarm;

            movement.SetNormalSpeed();
            
            if (randomPosition.HasValue)
                movement.SetPositionToMove(randomPosition.Value);
            else
                RandomMove();
        }
        public override void Update()
        {
            timeToSearch -= Time.deltaTime;
            if (timeToSearch < 0)
            {
                pda.PopState();
                return;
            }

            if (movement.HasReachedDestination())
                RandomMove();
        }
        void RandomMove()
        {
            var randomDirection = Random.insideUnitSphere * maxDistance;
            randomDirection += targetTransform.position;
            NavMesh.SamplePosition(randomDirection, out var hit, maxDistance, 1);
            randomPosition = hit.position;
            movement.SetPositionToMove(randomPosition.Value);
        }
        public override void Disable()
        {
            detection.OnTargetsChanged -= CheckTargets;
            alarmReceiver.OnAlarmReceived -= CheckAlarm;
        }
        public override void Exit() { }

        void CheckAlarm(AlarmData alarmData)
        {
            if (alarmData.targetTransform)
            {
                pda.PopState();
                pda.PushState(new EnemyShootState(pda, alarmData.targetTransform, 10f, 3f, 5f));
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

            pda.PopState();
            pda.PushState(new EnemyShootState(pda, selectedTarget.Value.Key, 10f, 3f, 5f));
        }
    }
}