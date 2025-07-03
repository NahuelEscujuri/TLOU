using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Automata
{
    public class EnemyRandomState : State
    {
        float timeToRandom;
        TargetsDetectionController detection;
        IAMovementController movement;
        Vector3? randomPosition;
        public EnemyRandomState(PushDownAutomata pda, float timeToRandom) : base(pda) 
        {
            this.timeToRandom = timeToRandom;
        }

        public override void Enter()
        {
            detection = pda.Context.targetDetection;
            movement = pda.Context.movement;
        }
        public override void Enable()
        {
            detection.OnTargetsChanged += CheckTargets;
            movement.SetNormalSpeed();

            if(randomPosition.HasValue)
               movement.SetPositionToMove(randomPosition.Value);
            else
                RandomMove();
        }
        public override void Update()
        {
            timeToRandom -= Time.deltaTime;

            if (timeToRandom < 0)
            {
                pda.PopState();
                return;
            }

            if (movement.HasReachedDestination())
                RandomMove();
        }
        void RandomMove()
        {
            var randomDirection = Random.insideUnitSphere * 5;
            randomDirection += movement.transform.position;
            NavMesh.SamplePosition(randomDirection, out var hit, 5, 1);
            randomPosition = hit.position;
            movement.SetPositionToMove(randomPosition.Value);
        }
        public override void Disable() => detection.OnTargetsChanged -= CheckTargets;
        public override void Exit(){}    
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
            pda.PushState(new EnemyChaseState(pda, selectedTarget.Value.Key));       
        }
    }
}