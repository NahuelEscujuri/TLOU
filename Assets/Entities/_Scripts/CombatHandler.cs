using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(MovementController))]
public class CombatHandler : MonoBehaviour
{
    WeaponController weaponController;
    MovementController movementController;
    [SerializeField]BodyDirection bodyDirection;

    [SerializeField] Collider attackColliderDetector;
    [SerializeField] string targetTag = "default";

    Transform currentTarget;

    void Awake()
    {
        bodyDirection = GetComponent<BodyDirection>();
        weaponController = GetComponent<WeaponController>();
        movementController = GetComponent<MovementController>();
    }

    void OnEnable()
    {
        weaponController.OnAction1 += Action1;
        weaponController.OnAction2 += Action2;
    }

    void OnDisable()
    {
        weaponController.OnAction1 -= Action1;
        weaponController.OnAction2 -= Action2;
    }

    #region Actions
    void Action1(HeldWeapon heldWeapon)
    {
        // Este metodo va a ser llamado cada vez que se ejecute Action1 payormente reprecentado como "click izquierdo"
        switch (heldWeapon)
        {
            case HeldWeapon.none:
                EnterMeleeAttack(HeldWeapon.none);
                break;

            case HeldWeapon.light_melee_blunt_weapon:
                EnterMeleeAttack(HeldWeapon.light_melee_blunt_weapon);
                break;

        }
    }

    void Action2(HeldWeapon heldWeapon)
    {

    }
    #endregion

    #region Melee methods
    [Header("Melee")]

    [SerializeField] float inputTimeWindow = .8f;

    bool isAttacking = false;
    bool queuedInput = false;

    public event Action<int, HeldWeapon, Transform> OnEnterMeleAttack;
    public event Action OnUpdateMeleAttack;
    public event Action<TypeDamangerResive,int , Transform> OnMeleAttack;
    public event Action OnExitMeleAttack;

    Coroutine comboResetRoutine = null;

    int comboStep = 0;
    [SerializeField] int maxCombo = 3;

    public void EnterMeleeAttack(HeldWeapon heldWeapon)
    {
        // Se va a ejecutar no bien se intente hacer un ataque
        if (isAttacking == true)
        {
            // va a guardar que se intento golpear
            bool queuedInput = true;
            return;
        }

        // Busca si hay un enemigo en el rango de ataque
        ChangeCurrentTarget(GetFirstCollidingWithTag(attackColliderDetector, targetTag)?.transform);

        // En caso de tenerlo hace que el jugador mire a su direcion
        if(currentTarget) bodyDirection.StartRedirectToTransform(currentTarget);

        // Hace que se quede quieto 
        movementController.ChangeMoveState(false);

        // Cancelar reseteo pendiente
        if (comboResetRoutine != null)
        {
            StopCoroutine(comboResetRoutine);
            comboResetRoutine = null;
        }

        // Avanzar índice de combo
        comboStep++;
        if (comboStep > maxCombo)
            comboStep = 1;

        // Disparar evento para animación
        Debug.Log(currentTarget);
        OnEnterMeleAttack?.Invoke(comboStep, heldWeapon, currentTarget);
        isAttacking = true;
        queuedInput = false;
    }

    public void MeleeAttack()
    {
        // Aca es donde se define si se le hace daño al enemigo
        if (currentTarget)
        {
            TypeDamangerResive finalTypeDamanger = TypeDamangerResive.up_right;

            switch (comboStep)
            {
                case 1:
                    finalTypeDamanger = TypeDamangerResive.up_front;
                    break; 
                case 2:
                    finalTypeDamanger = TypeDamangerResive.up_left_front;
                    break;
                case 3:
                    finalTypeDamanger = TypeDamangerResive.up_right;
                    break;
            }

            currentTarget.GetComponent<DamageHandler>().ResiveDamange(finalTypeDamanger, 35, transform);
            OnMeleAttack?.Invoke(finalTypeDamanger, 35, transform);
        }
    }

    void ExitMeleeAttack()
    {
        OnExitMeleAttack?.Invoke();
    }

    public void StartApproachToTarget(float distance = 1, float time = .3f)
    {
        OnUpdateMeleAttack?.Invoke();
        if (currentTarget) movementController.StartMoveToTarget(currentTarget.position, distance, time);
    }

    public void StartTimeWindows()
    {
        // Abre una ventan de oportunidad para volver a golpear
        isAttacking = false;
        movementController.ChangeMoveState(true);
        comboResetRoutine = StartCoroutine(ComboResetCoroutine());
    }

    IEnumerator ComboResetCoroutine()
    {
        ChangeCurrentTarget(null);
        yield return new WaitForSeconds(inputTimeWindow);
        ResetMeleeAttacks();
        comboResetRoutine = null;
    }

    public void ResetMeleeAttacks()
    {
        comboStep = 0;
        queuedInput = false;
        isAttacking = false;
        ExitMeleeAttack();
    }

    void ChangeCurrentTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    #endregion

    GameObject GetFirstCollidingWithTag(Collider col, string tagFilter)
    {
        // Usamos un OverlapBox que coincide con la caja envolvente del collider
        Vector3 center = col.bounds.center;
        Vector3 halfExt = col.bounds.extents;
        Quaternion rot = col.transform.rotation;

        // LayerMask opcional: por defecto ~0 = todas las capas
        LayerMask mask = ~0;

        // Detectamos todos los colliders que se solapan
        Collider[] hits = Physics.OverlapBox(
            center,
            halfExt,
            rot,
            mask,
            QueryTriggerInteraction.Collide
        );

        foreach (var h in hits)
        {
            if (h == col)
                continue;           // ignoramos el propio collider
            if (h.CompareTag(tagFilter))
                return h.gameObject; // devolvemos el primero que cumpla
        }

        return null; // ninguno cumplió la condición
    }

}
