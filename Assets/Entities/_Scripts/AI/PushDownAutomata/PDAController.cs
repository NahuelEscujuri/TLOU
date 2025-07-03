using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Automata
{
    public class PDAController : MonoBehaviour, ICoroutineGenerator
    {
        public event Action<State> OnCurrentStateChanged;

        [Header("References")]
        [SerializeField] TargetsDetectionController detectionController;
        [SerializeField] NavMeshAgent navMeshAgent;
        [SerializeField] IAMovementController movementController;
        [SerializeField] Transform[] patrolPoints;
        [SerializeField] EnemyAimSystem enemyAimSystem;
        [SerializeField] AlarmReceiver alarmReceiver;
        [SerializeField] AlarmEmiter alarmEmiter;
        CoversHandler coverHandler;
        PushDownAutomata pda;

        private void Awake()
        {
            coverHandler = FindAnyObjectByType<CoversHandler>();
            pda = new PushDownAutomata(new Context(detectionController, navMeshAgent, movementController, enemyAimSystem, this,
                patrolPoints, alarmReceiver, alarmEmiter, coverHandler));
            pda.OnCurrentStateChanged += InvokeCurrentStateChanged;
        }
        private void Start() => pda.PushState(new EnemyPatrolState(pda));
        private void OnDestroy()
        {
            pda.OnCurrentStateChanged -= OnCurrentStateChanged;
            pda.Dispose();
        }
        void InvokeCurrentStateChanged(State state) => OnCurrentStateChanged?.Invoke(state);
        private void Update() => pda.Update();
        public Coroutine NewCorrutine(IEnumerator enumerator) => StartCoroutine(enumerator);
        public void EndCorrutine(Coroutine coroutine) => StopCoroutine(coroutine);
    }
}