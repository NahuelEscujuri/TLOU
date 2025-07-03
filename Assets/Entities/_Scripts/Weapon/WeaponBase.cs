using System;
using System.Collections;
using UnityEngine;

namespace Automata
{
    public class WeaponBase : MonoBehaviour, IWeapon
    {
        public event Action OnNeedReload;
        public event Action OnReloeadCompleted;
        public float CurrentAmmoRate => currentMagazineAmmo / data.magazineSize;
        public bool HasAmmo => currentTotalAmmo > 0 || currentMagazineAmmo > 0;
        public Transform ShootTransform => shootTransform;
        [SerializeField] Transform shootTransform;
        WeaponData data;
        int currentMagazineAmmo;
        int currentTotalAmmo;

        public void SetData(WeaponData weaponData)
        {
            this.data = weaponData; 
            currentMagazineAmmo = data.magazineSize;
            currentTotalAmmo = data.maxAmmo;
        }

        #region reload
        public bool NeedReload => currentMagazineAmmo <= 0;
        public bool IsReloading => reloadCoroutine != null;
        Coroutine reloadCoroutine;
        public void Reload()
        {
            if (IsReloading) return;

            reloadCoroutine = StartCoroutine(ReloadCoroutine());
        }
        IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(data.reloadTime);
            currentMagazineAmmo = Mathf.Min(data.magazineSize, currentTotalAmmo);
            currentTotalAmmo -= currentTotalAmmo;
            Debug.Log($"current total ammo {currentTotalAmmo} current magazine {currentMagazineAmmo}");
            reloadCoroutine = null;
        }
        #endregion

        #region Shoot
        Coroutine shootCoroutine;
        public void Shoot()
        {
            if (IsReloading || shootCoroutine != null || NeedReload) return;

            shootCoroutine = StartCoroutine(ShootCoroutine());
        }
        IEnumerator ShootCoroutine()
        {
            var projectileInstance = Instantiate(data.proyectilePrefab, shootTransform.position, shootTransform.rotation);
            projectileInstance.SetActive(true);
            projectileInstance.GetComponent<Rigidbody>().AddForce(shootTransform.forward * data.proyectileSpeed,ForceMode.Impulse);
            Destroy(projectileInstance, 3f); // Destruimos el proyectil despues de 3 segundos, para prueba
            currentMagazineAmmo--;
            yield return new WaitForSeconds(1f / data.fireRate);
            shootCoroutine = null;
        }
        #endregion

        #region equip/unequip
        public void Equip(Transform slot)
        {
            transform.SetParent(slot);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
        public void Unequip()
        {
            transform.SetParent(null);
        }
        #endregion
    }
}