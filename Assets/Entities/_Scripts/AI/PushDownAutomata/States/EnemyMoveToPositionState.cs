using System.Collections.Generic;
using UnityEngine;

namespace Automata
{
    public class EnemyMoveToPositionState : State
    {
        IAMovementController movement;
        TargetsDetectionController detection;
        Vector3 positionToMove;
        public EnemyMoveToPositionState(PushDownAutomata pda, Vector3 positionToMove) : base(pda)
        {
            this.positionToMove = positionToMove;
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
            movement.SetPositionToMove(positionToMove);
        }
        public override void Update()
        {
            if (movement.HasReachedDestination())
            {
                pda.PopState();
                pda.PushState(new EnemyRandomState(pda, 5f));
                return;
            }
        }
        public override void Disable() => detection.OnTargetsChanged -= CheckTargets;
        public override void Exit() { }
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