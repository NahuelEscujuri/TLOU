using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidSFXHandler : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> slapList;
    [SerializeField] float minPitch = .8f;
    [SerializeField] float maxPitch = 1.2f;

    [Header("Components")]
    [SerializeField] DamageHandler damageHandler;


    void OnEnable()
    {
        damageHandler.OnResiveDamange += ResiveDamange;
    }

    void OnDisable()
    {
        damageHandler.OnResiveDamange -= ResiveDamange;
    }

    void ResiveDamange(TypeDamangerResive typeDamanger, float damange, Transform target)
    {
        AudioClip clip = slapList[Random.Range(0, slapList.Count)];
        PlayOneShotRandomPitch(clip);
    }

    void PlayOneShotRandomPitch(AudioClip audio)
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(audio);
    }
}
