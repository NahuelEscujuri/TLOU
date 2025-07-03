using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollToggle : MonoBehaviour
{
    public bool active = false;
    public event Action<bool> OnChangeState;
    [SerializeField] Component[] desactiveComponents;
    [SerializeField] GameObject[] bodyParts;

    void Start()
    {
        if (active) ActiveRagdoll();
        else DesactiveRagdoll();
    }

    public void ActiveRagdoll()
    {
        active = true;
        // Desactiva los componentes 
        for (int i = 0; i < desactiveComponents.Length; i++)
        {
            var myComponent = desactiveComponents[i];

            // Verificar si tiene la propiedad 'enabled'
            var enabledProperty = myComponent.GetType().GetProperty("enabled");

            // Si la tiene la desactiva
            if (enabledProperty != null && enabledProperty.CanWrite)
            {
                enabledProperty.SetValue(myComponent, false);
            }
        }

        for (int i = 0; i < bodyParts.Length; i++)
        {
            Collider col = bodyParts[i].GetComponent<Collider>();
            Rigidbody rb = bodyParts[i].GetComponent<Rigidbody>();

            rb.isKinematic = false;
            col.isTrigger = false;
        }

        if(OnChangeState != null) OnChangeState.Invoke(active);
    }

    public void DesactiveRagdoll()
    {
        active = false;
        //Debug.Log($"DesactiveRagdoll - {active} | {gameObject.name}");
        // Activa los componentes 
        for (int i = 0; i < desactiveComponents.Length; i++)
        {
            var myComponent = desactiveComponents[i];

            // Verificar si tiene la propiedad 'enabled'
            var enabledProperty = myComponent.GetType().GetProperty("enabled");

            // Si la tiene la activa
            if (enabledProperty != null && enabledProperty.CanWrite)
            {
                enabledProperty.SetValue(myComponent, true);
             
            }
        }

        for (int i = 0; i < bodyParts.Length; i++)
        {
            Collider col = bodyParts[i].GetComponent<Collider>();
            Rigidbody rb = bodyParts[i].GetComponent<Rigidbody>();

            rb.isKinematic = true;
            col.isTrigger = true;
        }

        if (OnChangeState != null) OnChangeState.Invoke(active);
    }

    public GameObject[] GetBodyParts() => bodyParts;
}
