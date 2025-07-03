using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public event Action<TypeDamangerResive, float, Transform> OnResiveDamange;

    public void ResiveDamange(TypeDamangerResive typeDamanger, float damange, Transform target)
    {
        OnResiveDamange?.Invoke(typeDamanger, damange, target);
    }
}
public enum TypeDamangerResive
{
    up_front,
    up_left,
    up_left_front,
    up_right,
    up_right_front
}
