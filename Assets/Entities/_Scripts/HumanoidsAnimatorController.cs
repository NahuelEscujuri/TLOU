using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidsAnimatorController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] DamageHandler damageHandler;

    private void OnEnable()
    {
        damageHandler.OnResiveDamange += ResiveDamange;
    }

    private void OnDisable()
    {
        damageHandler.OnResiveDamange -= ResiveDamange;
    }

    void ResiveDamange(TypeDamangerResive typeDamanger, float damange, Transform target)
    {
        animator.SetTrigger($"humanoid_alert_hit_{typeDamanger}");

        // mira a quien lo golpeo
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // Eliminar inclinación vertical

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
    }
}
