using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class BoxingBag : MonoBehaviour
{
    [SerializeField] float RotateDuration = .3f;
    [SerializeField] DamageHandler damageHandler;

    [Header("Audio")]
    [SerializeField] AudioClip audioClip;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float minPitch = .8f;
    [SerializeField] float maxPitch = 1.8f;

    [Tooltip("Curva que controle la transición. Debe empezar en 0, subir hasta 1 y volver a 0.")]
    public AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 0);

    void OnEnable()
    {
        if (!damageHandler) damageHandler = GetComponent<DamageHandler>();
        damageHandler.OnResiveDamange += ResiveDamange;
    }

    void ResiveDamange(TypeDamangerResive typeDamanger, float damange, Transform target)
    {
        StopAllCoroutines();

        PlayOneShotRandomPitch(audioClip);

        Vector3 finalRotation = Vector3.zero;

        switch (typeDamanger)
        {
            case TypeDamangerResive.up_front:
                finalRotation = new Vector3(-180, 0, 0);
                break;

            case TypeDamangerResive.up_right:
                finalRotation = new Vector3(0, 0, -180);
                break;

            case TypeDamangerResive.up_right_front:
                finalRotation = new Vector3(-180, 0, -180);
                break;

            case TypeDamangerResive.up_left:
                finalRotation = new Vector3(0, 0, 180);
                break;

            case TypeDamangerResive.up_left_front:
                finalRotation = new Vector3(-180, 0, 180);
                break;
        }

        StartRotation(finalRotation, target);
    }

    void PlayOneShotRandomPitch(AudioClip audio)
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(audio);
    }

    #region Rotation Methos
    void StartRotation(Vector3 maxRotationEuler, Transform target)
    {
        LookAtTarget(transform, target);
        StartCoroutine(RotateWithCurve(maxRotationEuler));
    }

    IEnumerator RotateWithCurve(Vector3 maxRotationEuler)
    {
        // Guardamos la rotación original
        Quaternion originalRot = transform.rotation;
        float elapsed = 0f;

        while (elapsed < RotateDuration)
        {
            // Normalizamos el tiempo a [0,1]
            float t = elapsed / RotateDuration;

            // Evaluamos la curva en t. Debe dar 0 al inicio y al final, 1 en el pico.
            float curveValue = rotationCurve.Evaluate(t);

            // Calculamos la rotación objetivo en base al valor de la curva
            Quaternion targetRot = originalRot * Quaternion.Euler(maxRotationEuler * curveValue);

            // Aplicamos la rotación interpolada
            transform.rotation = targetRot;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Al terminar, restauramos exactamente la rotación original
        transform.rotation = originalRot;
    }
    #endregion

    void LookAtTarget(Transform self, Transform target)
    {
        Vector3 direction = target.position - self.position;
        direction.y = 0; // Eliminar inclinación vertical

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        self.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0); // Solo eje Y
    }


}
