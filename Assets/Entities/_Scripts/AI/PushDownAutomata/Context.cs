using UnityEngine;
using UnityEngine.AI;

namespace Automata
{
    public struct Context
    {
        public TargetsDetectionController targetDetection;
        public NavMeshAgent nav;
        public IAMovementController movement;
        public ICoroutineGenerator coroutineGenerator;
        public Transform[] patrolPoints;
        public EnemyAimSystem aim;
        public AlarmReceiver alarmReceiver;
        public AlarmEmiter alarmEmiter;
        public ICoverHandler coverSystem;

        public Context(TargetsDetectionController detectionController, NavMeshAgent navMeshAgent, IAMovementController movementController, EnemyAimSystem aimSystem, 
            ICoroutineGenerator coroutineGenerator, Transform[] patrolPoints, AlarmReceiver alarmReceiver, AlarmEmiter alarmEmiter, ICoverHandler coverSystem)
        {
            this.targetDetection = detectionController;
            this.coroutineGenerator = coroutineGenerator;
            this.nav = navMeshAgent;
            this.movement = movementController;
            this.patrolPoints = patrolPoints;
            this.aim = aimSystem;
            this.alarmReceiver = alarmReceiver;
            this.alarmEmiter = alarmEmiter;
            this.coverSystem = coverSystem;
        }
    }
    public struct PersonalValue
    {

    } 
}