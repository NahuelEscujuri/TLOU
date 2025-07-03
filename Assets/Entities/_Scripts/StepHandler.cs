using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepHandler : MonoBehaviour
{
    [SerializeField] List<AudioClip> footstepSound;
    [SerializeField] AudioSource footstepAudioSource;

    public void PlayFoodStep()
    {
        footstepAudioSource.PlayOneShot(footstepSound[UnityEngine.Random.Range(0, footstepSound.Count)]);
    }

}
