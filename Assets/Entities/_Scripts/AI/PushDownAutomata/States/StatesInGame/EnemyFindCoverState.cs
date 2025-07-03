using UnityEngine;

namespace Automata
{
    public class EnemyFindCoverState : State
    {
        readonly Transform targetTransform;
        readonly float maxDistance;
        readonly float interval;
        float timer;
        IAMovementController movement;
        ICoverHandler coverHandler;
        CoverPoint currentCover;

        public EnemyFindCoverState(PushDownAutomata pda, Transform targetTransform, float maxDistance, float interval) : base(pda)
        {
            this.targetTransform = targetTransform;
            this.maxDistance = maxDistance;
            this.interval = interval;
        }
        public override void Enter()
        {
            movement = pda.Context.movement;
            coverHandler = pda.Context.coverSystem;
        }
        public override void Enable()
        {
            movement.SetRun();
            currentCover = null;
            timer = 0f;
        }
        public override void Update()
        {
            CheckCovers();
            MoveToCover();
        }
        void CheckCovers()
        {
            timer -= Time.deltaTime;
            if (timer > 0) return;

            timer = interval;
            currentCover = null;  
            var targetPosition = targetTransform.position;
            var covers = coverHandler.GetCoverPointsInArea(movement.transform.position, maxDistance);

            foreach (var cover in covers)
            { 
                Vector3 directionToTarget = (targetPosition - cover.pointTransform.position).normalized;
                float alignment = Vector3.Dot(-cover.pointTransform.forward, directionToTarget);

                if (alignment > 0.75f)
                {
                    currentCover = cover;
                    break;
                }
            }
            if(!currentCover)
                pda.PopState();
        }
        void MoveToCover()
        {
            if (!currentCover) return;

            movement.SetPositionToMove(currentCover.transform.position);
            if(movement.HasReachedDestination())
                pda.PopState();
        }
        public override void Disable() { }
        public override void Exit() { }
    }
}