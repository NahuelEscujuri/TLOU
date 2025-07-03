using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    public string levelName;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(levelName);        
    }
}
