using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] WeaponController weaponController;
    CombatHandler combatHandler;
    PlayerMovement playerMovement;
    BodyDirection bodyDirection;

    void Awake()
    {
        combatHandler = GetComponent<CombatHandler>();
        weaponController = GetComponent<WeaponController>();
        playerMovement = GetComponent<PlayerMovement>();
        bodyDirection = GetComponent<BodyDirection>();
        animator.SetBool("_tense_mode", true);
    }

    void OnEnable()
    {
        weaponController.OnAction1 += ActionWeapon1;
        weaponController.OnAction2 += ActionWeapon2;

        // Combat melee
        combatHandler.OnEnterMeleAttack += EnterMeleAttack;
        combatHandler.OnUpdateMeleAttack += UpdateMeleAttack;
        combatHandler.OnExitMeleAttack += ExitMeleAttack;
    }
    void OnDisable()
    {
        weaponController.OnAction1 -= ActionWeapon1;
        weaponController.OnAction2 -= ActionWeapon2;

        // Combat melee
        combatHandler.OnEnterMeleAttack -= EnterMeleAttack;
        combatHandler.OnUpdateMeleAttack -= UpdateMeleAttack;
        combatHandler.OnExitMeleAttack -= ExitMeleAttack;
    }

    void Update()
    {
        animator.SetFloat("move_x", bodyDirection.CurrentVelocity.x);
        animator.SetFloat("move_y", bodyDirection.CurrentVelocity.y);
        animator.SetBool("run", playerMovement.IsRun());
        animator.SetBool("player_moving", playerMovement.IsMoving());
    }

    void ActionWeapon1(HeldWeapon heldWeapon)
    {
        /*switch(heldWeapon)
        {
            case HeldWeapon.none:
                //animator.SetTrigger("attack_" + 1);
                break;

            case HeldWeapon.light_melee_blunt_weapon:
                animator.Play("light_melee_blunt_weapon_0");
                break;
        }*/
    }

    void ActionWeapon2(HeldWeapon heldWeapon)
    {

    }

    #region Melee Attack
    void EnterMeleAttack(int index, HeldWeapon heldWeapon, Transform target)
    {
        switch (heldWeapon)
        {
            case HeldWeapon.none:
                animator.SetTrigger("attack_" + index);
                EnableRootMotion();
                break;

            case HeldWeapon.light_melee_blunt_weapon:
                animator.SetTrigger("attack_light_melee_blunt_weapon_" + index);
                EnableRootMotion();
                break;
        }
    }
    void UpdateMeleAttack()
    {
        //EnableRootMotion();
    }

    void ExitMeleAttack()
    {
        DisableRootMotion();
    }
    #endregion

    public void ResetTriggers(string trigger, int cant)
    {
        for (int i = 1; i <= cant; i++)
        {
            animator.ResetTrigger(trigger + i);
        }
    }



    #region Root Motion
    public void EnableRootMotion() => animator.applyRootMotion = true;

    public void DisableRootMotion()
    {
        animator.applyRootMotion = false;
        AlingRootWithBody();
    }

    public void AlingRootWithBody()
    {
        transform.position = new Vector3(bodyDirection.GetBody.position.x, transform.position.y, bodyDirection.GetBody.position.z);
        bodyDirection.GetBody.localPosition = new Vector3(0, -transform.localScale.y, 0);
    }
    #endregion
}
