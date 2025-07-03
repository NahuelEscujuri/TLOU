using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraHandler : MonoBehaviour
{
    [SerializeField] CameraController camController;
    CombatHandler combatHandler;

    void OnEnable()
    {
        if(!combatHandler) combatHandler = GetComponent<CombatHandler>();
        combatHandler.OnEnterMeleAttack += EnterFightMode;
        combatHandler.OnMeleAttack += MeleeAttackShake;
        combatHandler.OnExitMeleAttack += ExitFightMode;
    }

    void OnDisable()
    {
        combatHandler.OnEnterMeleAttack -= EnterFightMode;
        combatHandler.OnMeleAttack -= MeleeAttackShake;
    }

    void EnterFightMode(int index, HeldWeapon heldWeapon, Transform target)
    {
        if (!target) return;
        camController.EnableCameraFight();
        camController.ChangeTargets(target);
    }

    void MeleeAttackShake(TypeDamangerResive typeDamanger, int damange, Transform target)
    {

        switch (typeDamanger)
        {
            case TypeDamangerResive.up_front:
                CameraShake(new Vector3(.08f, 0, .1f), .5f);
                break;

            case TypeDamangerResive.up_right:
                CameraShake(new Vector3(-.1f, 0, 0), .5f);
                break;

            case TypeDamangerResive.up_right_front:
                CameraShake(new Vector3(-.1f, 0, .07f), .5f);
                break;

            case TypeDamangerResive.up_left_front:
                CameraShake(new Vector3(.1f, 0, 0), .5f);
                break;
        }


    }

    void ExitFightMode()
    {
        camController.DisableCameraFight();
    }

    void CameraShake(Vector3 velocity, float duration = .5f)
    {
        camController.CameraShake(velocity, duration);
    }

}
