using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] HeldWeapon currentHeldWeapon;

    public event Action<HeldWeapon> OnAction1;
    public event Action<HeldWeapon> OnAction2;

    public void Action1() => OnAction1?.Invoke(currentHeldWeapon);
    public void Action2() => OnAction2?.Invoke(currentHeldWeapon);

    public void ChangeWeapon(HeldWeapon heldWeapon) => currentHeldWeapon = heldWeapon;

}

public enum HeldWeapon
{
    none,
    light_melee_blunt_weapon,
    heavy_melee_blunt_weapon,
}