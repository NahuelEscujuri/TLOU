using UnityEngine;

namespace Automata
{
    public class EnemyPanicState : State
    {
        readonly Transform targetTransform;
        IAMovementController movement;
        TargetsDetectionController detection;
        float time;
        bool isTargetLost;
        public EnemyPanicState(PushDownAutomata pda, Transform targetTransform, float time) : base(pda)
        {
            this.time = time;
            this.targetTransform = targetTransform;
        }
        public override void Enter()
        {
            detection = pda.Context.targetDetection;
            movement = pda.Context.movement;
        }
        public override void Enable()
        {
            detection.OnTargetsChanged += CheckTargets;
            movement.SetCanMove(false);
        }
        public override void Update()
        {
            time -= Time.deltaTime;
            if (time <= 0f)
            {
                pda.PopState();
                pda.PushState((isTargetLost) ? new EnemySearchState(pda, 50f, 3f, targetTransform) : new EnemyShootState(pda, targetTransform, 10f, 3f, 5f));
                return;
            }
            movement.SetPositionToMove(targetTransform.position);
        }
        public override void Disable()
        {
            movement.SetCanMove(true);
            detection.OnTargetsChanged -= CheckTargets;
        }
        public override void Exit() { }
        void CheckTargets() => isTargetLost = !detection.TargetsDetected.ContainsKey(targetTransform);
    }
}