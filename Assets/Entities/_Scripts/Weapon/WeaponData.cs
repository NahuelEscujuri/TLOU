using System;
using UnityEngine;

namespace Automata
{
    [Serializable]
    public struct WeaponData
    {
        public GameObject proyectilePrefab;
        public int magazineSize;
        public int maxAmmo;
        public float fireRate;
        public float proyectileSpeed;
        public float reloadTime;
    }
}