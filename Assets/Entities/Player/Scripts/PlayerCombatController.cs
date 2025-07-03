using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] WeaponController weaponController;

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) weaponController.Action1();
    }
}
