using UnityEngine;

namespace Automata
{
    public class EnemyShootState : State
    {
        readonly Transform targetTransform;
        readonly float angleGap;
        readonly float alarmInterval;
        readonly float minDistance;
        readonly float leaveStateGap = 2f;
        float targetLostTimer;
        float alarmTimer;
        TargetsDetectionController detection;
        IAMovementController movement;
        EnemyAimSystem aimSystem;
        AlarmEmiter alarmEmiter;
        public EnemyShootState(PushDownAutomata pda, Transform targetTransform, float angleGap, float alarmInterval, float minDistance) : base(pda)
        {
            this.targetTransform = targetTransform;
            this.alarmInterval = alarmInterval;
            this.angleGap = angleGap;
            this.minDistance = minDistance;
        }
        public override void Enter()
        {
            detection = pda.Context.targetDetection;
            movement = pda.Context.movement;
            aimSystem = pda.Context.aim;
            alarmEmiter = pda.Context.alarmEmiter;
        }
        public override void Enable()
        {
            movement.SetRun();
            alarmTimer = alarmInterval;
        }
        public override void Update()
        {
            Alarm();
            Move();
            Shoot();
            Reload();
            CheckTargetLost();
        }
        void CheckTargetLost()
        {
            if (detection.TargetsDetected.ContainsKey(targetTransform))
            {
                targetLostTimer = 0f;
            }
            else
            {
                targetLostTimer += Time.deltaTime;
                if (targetLostTimer >= leaveStateGap)
                {
                    pda.PopState();
                    pda.PushState(new EnemySearchState(pda, 50, 3, targetTransform));
                }
            }
        }
        void Alarm()
        {
            alarmTimer -= Time.deltaTime;
            if (alarmTimer > 0) return;
            alarmTimer = alarmInterval;
            alarmEmiter.EmitAlarm(new AlarmData() { state = AlarmData.State.Found, targetTransform = targetTransform });
        }
        void Move()
        {
            if (Vector3.Distance(targetTransform.position, movement.transform.position) < minDistance)
                movement.SetCanMove(false);
            else
                movement.SetCanMove(true);
                movement.SetNormalSpeed();
                movement.SetPositionToMove(targetTransform.position);
        }
        void Shoot()
        {
            aimSystem.UpdateAimToPosition(targetTransform.position);
            if (aimSystem.GetAimingPositionAngleDifference(targetTransform.position) > angleGap) return;

            aimSystem.Shoot();           
        }
        void Reload()
        {
            if(!aimSystem.HasAmmo || !aimSystem.NeedReaload) return;

            aimSystem.Reload();
        }
        public override void Exit() { }
    }
}