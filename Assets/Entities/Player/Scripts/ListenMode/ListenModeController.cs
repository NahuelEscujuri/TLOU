using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenModeController : MonoBehaviour
{
    [SerializeField] private KeyCode triggerKey = KeyCode.Q;
    [SerializeField] ListenMode listenMode;

    void Update()
    {
        if (Input.GetKeyDown(triggerKey)) listenMode.Activate();
        else if(Input.GetKeyUp(triggerKey)) listenMode.Desactive();
    }
}
