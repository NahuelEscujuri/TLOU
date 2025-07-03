using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    [SerializeField] CombatHandler combatHandler;
    
    public void StartTimeWindows()
    {
        combatHandler.StartTimeWindows();
    }

    public void StartMeleeAttck()
    {
        combatHandler.StartApproachToTarget(1f, .1f);
    }

    public void MeleeAttack()
    {
        combatHandler.MeleeAttack();
    }
}
