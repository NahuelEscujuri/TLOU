using System;
using UnityEngine;

namespace Automata
{
    public interface IWeapon: IShooteable, IEquippable 
    {
       
    }
    public interface IEquippable
    {
        void Equip(Transform slot);
        void Unequip();
    }

    public interface IShooteable
    {
        event Action OnNeedReload;
        event Action OnReloeadCompleted;

        float CurrentAmmoRate { get; }
        Transform ShootTransform { get; }
        bool HasAmmo { get; }
        bool NeedReload { get; }
        bool IsReloading { get; }
        void Shoot();
        void Reload();
    }
}