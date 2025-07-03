using System;
using UnityEngine;

namespace Automata
{
    public class EnemyAimSystem : MonoBehaviour
    {
        public IWeapon CurrenWeapon => currentWeapon;

        [SerializeField] Transform handTransform;
        [SerializeField] float rotationSpeed = 5f;
        [Tooltip("Limitación de angulo que puede inclinarse")]
        [SerializeField] float maxVerticalAngle = 30f;
        IWeapon currentWeapon;

        [Header("Debug")]
        [SerializeField] bool useDebug;
        [SerializeField] WeaponData debugWeaponData;
        [SerializeField] GameObject debugWeaponGO;
 
        private void Awake()
        {
            if (useDebug)
            {
                debugWeaponGO.SetActive(true);
                var debugWeaponBase = debugWeaponGO.GetComponent<WeaponBase>();
                debugWeaponBase.SetData(debugWeaponData);
                EquipWeapon(debugWeaponBase);
            }
        }

        public void EquipWeapon(IWeapon newWeapon)
        {
            if (currentWeapon == newWeapon) return;

            currentWeapon?.Unequip();
            currentWeapon = newWeapon;
            currentWeapon?.Equip(handTransform);
        }
        public void UnequipoCurrentWeapon()
        {
            currentWeapon?.Unequip();
            currentWeapon = null;
        }

        public void Shoot() => currentWeapon?.Shoot();
        public void Reload() => currentWeapon?.Reload();
        public bool NeedReaload => currentWeapon?.NeedReload ?? false;
        public bool HasAmmo => currentWeapon?.HasAmmo ?? false;

        /// <summary>
        /// Recibe la posicion a apuntar, eevuelve la diferencia de angulos entre el punto de lanzamiento del arma y la posicion a apuntar
        /// </summary>
        /// <param name="positionToAim"></param>
        /// <returns></returns>
        public float GetAimingPositionAngleDifference(Vector3 positionToAim)
        {
            // Obtén la posición del origen (disparo) y calcula la dirección al objetivo
            Vector3 shootPosition = currentWeapon.ShootTransform.position;
            Vector3 targetDirection = positionToAim - shootPosition;

            // Proyecta el vector al plano XZ eliminando la componente vertical
            targetDirection.y = 0;

            // Obtén y proyecta la dirección forward del arma sobre XZ
            Vector3 forwardDirection = currentWeapon.ShootTransform.forward;
            forwardDirection.y = 0;

            // Calcula el ángulo entre los dos vectores proyectados
            return Vector3.Angle(targetDirection, forwardDirection);
        }
        /// <summary>
        /// Inclina al personaje en base a la posicion a apuntar
        /// </summary>
        /// <param name="positionToAim"></param>
        public void UpdateAimToPosition(Vector3 positionToAim)
        {
            Vector3 direction = positionToAim - transform.position;
            direction.y = 0f; // evitamos rotación vertical (solo en plano XZ)

            if (direction.sqrMagnitude < 0.01f) return; // evitamos errores por dirección cero

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}