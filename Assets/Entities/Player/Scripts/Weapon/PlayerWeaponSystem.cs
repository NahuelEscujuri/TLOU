using System;
using UnityEngine;

namespace Automata
{
    public class PlayerWeaponSystem : MonoBehaviour
    {
        public IWeapon CurrentWeapon => currentWeapon;

        [SerializeField] Transform handTransform;
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

        public void UnequipCurrentWeapon()
        {
            currentWeapon?.Unequip();
            currentWeapon = null;
        }

        public void Shoot()
        {
            currentWeapon?.Shoot();
        }

        public void Reload()
        {
            currentWeapon?.Reload();
        }

        public bool NeedReload => currentWeapon?.NeedReload ?? false;

        public bool HasAmmo => currentWeapon?.HasAmmo ?? false;

        public float CurrentAmmoRate => currentWeapon?.CurrentAmmoRate ?? 0f;

        public Transform ShootTransform => currentWeapon?.ShootTransform;
    }
}

