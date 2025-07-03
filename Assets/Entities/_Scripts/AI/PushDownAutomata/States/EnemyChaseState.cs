using UnityEngine;

namespace Automata
{
    public class EnemyChaseState : State
    {
        readonly Transform targetTransform;
        TargetsDetectionController detection;
        IAMovementController movement;
        public EnemyChaseState(PushDownAutomata pda, Transform targetTransform) : base(pda) 
        {
            this.targetTransform = targetTransform;
        }
        public override void Enter()
        {
            detection = pda.Context.targetDetection;
            movement = pda.Context.movement;
        }
        public override void Enable()
        {
            movement.SetRun();
            detection.OnTargetsChanged += CheckTargets;
        }
        public override void Update() => movement.SetPositionToMove(targetTransform.position);
        public override void Disable() => detection.OnTargetsChanged -= CheckTargets;
        public override void Exit() { }
        void CheckTargets()
        {
            if (detection.TargetsDetected.ContainsKey(targetTransform)) return;

            pda.PopState();
            pda.PushState(new EnemyMoveToPositionState(pda, movement.positionToMove));
        }
    }
}