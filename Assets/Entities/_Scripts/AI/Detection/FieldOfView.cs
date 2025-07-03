using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

namespace Automata
{
    public class FieldOfView : MonoBehaviour
    {
        [Tooltip("Evento que se lanza cuando se detecta un objetivo")]
        public event Action<Transform> OnTargetDetected;
        [Tooltip("Evento que se lanza cuando se pierde de vista un objetivo")]
        public event Action<Transform> OnTargetLost;

        [Tooltip("Radio del campo de visión")]
        public float viewRadius = 10f;
        [Tooltip("Ángulo del campo de visión")]
        [Range(0, 360), SerializeField] float viewAngle = 45f;
        [Header("Close circle")]
        [SerializeField] bool useCloseCircle;
        [SerializeField] float closeCircleRadius = 2f;

        [SerializeField] LayerMask targetMask;
        [SerializeField] LayerMask obstacleMask;
        public HashSet<Transform> VisibleTargets => visibleTargets;
        HashSet<Transform> visibleTargets = new();
        Coroutine findTargetsCor;

        #region detection
        private void OnEnable() => findTargetsCor = StartCoroutine(FindTargetsWithDelay(.2f));
        private void OnDisable() => StopCoroutine(findTargetsCor);
        IEnumerator FindTargetsWithDelay(float delay)
        {
            var wait = new WaitForSeconds(delay);
            while (true)
            {
                yield return wait;
                FindVisibleTargets();
            }
        }
        void FindVisibleTargets()
        {
            HashSet<Transform> newVisibleTargets = new(); // Nueva lista en lugar de limpiar directamente

            // Encuentra todos los colliders dentro del radio de visión que están en la capa de objetivo
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            var circleCenter = (useCloseCircle) ? transform.position + transform.forward * closeCircleRadius : Vector3.zero;
            
            foreach (Collider target in targetsInViewRadius)// Recorre cada objetivo encontrado
            {
                Transform targetTransform = target.transform; // Obtiene la transform del objetivo
                Vector3 targetCenter = target.bounds.center; // Obtiene el centro del target en base a su collider
                Vector3 dirToTarget = (targetCenter - transform.position).normalized; // Calcula la dirección al objetivo

                if (Vector3.Angle(transform.forward, dirToTarget) > viewAngle * .5f)// Comprueba si el objetivo está por fuera del angulo de visión
                {
                    if (!useCloseCircle) continue; // si no comprueba circulo cercano se descarta el target

                    if (Vector3.Distance(circleCenter, targetCenter) > closeCircleRadius) continue; // si está por fuera del circulo se descarta
                }
                
                float distanceToTarget = Vector3.Distance(transform.position, targetCenter); // Calcula la distancia al objetivo

                if (Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask)) continue;  // Comprueba si hay obstáculos entre el enemigo y el objetivo

                newVisibleTargets.Add(targetTransform); // Añade el objetivo a la lista de visibles

                if (!visibleTargets.Contains(targetTransform))
                {
                    OnTargetDetected?.Invoke(targetTransform); // Lanza el evento cuando se detecta un nuevo objetivo
                    Debug.Log("target detectada: " + targetTransform.name);
                }
            }

            // Lanza el evento OnTargetLost para los objetivos que ya no son visibles
            foreach (Transform previousTarget in visibleTargets)
            {
                if (!newVisibleTargets.Contains(previousTarget))
                {
                    OnTargetLost?.Invoke(previousTarget); // Lanza el evento cuando se pierde de vista un objetivo
                    Debug.Log("target perdida: " + previousTarget.name);
                }
            }

            visibleTargets = newVisibleTargets;
        }
        #endregion

        void OnDrawGizmos()
        {
            // Dibuja el radio general de visión en amarillo
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, viewRadius);

            // Calcula las direcciones que marcan los bordes del ángulo de visión
            Vector3 rightBoundary = Quaternion.Euler(0, viewAngle * 0.5f, 0) * transform.forward;
            Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle * 0.5f, 0) * transform.forward;
            // Dibuja las líneas desde la posición actual hasta el límite de visión (multiplicado por viewRadius)
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);

            if (useCloseCircle)
            {
                // Calcula el centro del círculo cercano (close circle) adelantado en la dirección del forward
                Vector3 circleCenter = transform.position + transform.forward * closeCircleRadius;
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(circleCenter, closeCircleRadius);
            }

            // Dibuja líneas en rojo hacia los objetivos actualmente visibles
            Gizmos.color = Color.red;
            foreach (Transform visibleTarget in visibleTargets)
            {
                Gizmos.DrawLine(transform.position, visibleTarget.position);
            }
        }
    }
}