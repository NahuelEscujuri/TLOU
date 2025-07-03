using System;
using UnityEngine;
using UnityEngine.AI;

namespace Automata
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class IAMovementController : MonoBehaviour
    {
        public enum Stance
        {
            Standing,
            Crouching,
            Prone
        }

        [SerializeField] NavMeshAgent navMeshAgent;
        [Header("Movement Settings")]
        public float baseMoveSpeed = 2f;
        public float baseAcceleration = 2f;
        public float runMoveMultiplier = 2f;
        public float runAccMultiplier = 2f;
        [HideInInspector] public Vector3 positionToMove;

        private void Awake() => navMeshAgent = GetComponent<NavMeshAgent>();
        void Update()
        {
            GoToPositionMovement();
            SetRotationValues();
        }
        public bool HasReachedDestination()
            => !navMeshAgent.pathPending
                   && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance
                   && (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f);

        #region speed
        public void SetCanMove(bool value)
        {
            if (navMeshAgent.isActiveAndEnabled == true)
            {
                navMeshAgent.isStopped = !value;
            }
        }
        public void SetRun()
        {
            navMeshAgent.speed = baseMoveSpeed * runMoveMultiplier;
            navMeshAgent.acceleration = baseAcceleration * runAccMultiplier;
        }
        public void SetNormalSpeed()
        {
            navMeshAgent.speed = baseMoveSpeed;
            navMeshAgent.acceleration = baseAcceleration;
        }
        #endregion

        #region move
        public void SetPositionToMove(Vector3 position) => positionToMove = position;
        void GoToPositionMovement() => navMeshAgent.SetDestination(positionToMove);
        #endregion

        #region rotation logic
        [HideInInspector] public float deltaYRotation;
        readonly float smoothingSpeed = 4.0f; // Velocidad de suavización
        void SetRotationValues()
        {
            // Dirección actual del objeto
            Vector3 currentDirection = transform.forward;

            // Dirección hacia el objetivo de navegación
            Vector3 targetDirection = (navMeshAgent.steeringTarget - transform.position).normalized;

            // Calcular el ángulo entre la dirección actual y la dirección del objetivo
            float targetAngle = Vector3.SignedAngle(currentDirection, targetDirection, Vector3.up);

            // Interpolación suave entre el ángulo actual y el ángulo objetivo
            deltaYRotation = Mathf.Lerp(deltaYRotation, targetAngle, Time.deltaTime * smoothingSpeed);
        }
        #endregion
    }
}