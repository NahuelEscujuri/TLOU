using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartsDetectorDamange : MonoBehaviour
{
    [SerializeField] string tagDetector = "Damange";
    [SerializeField] string tagBodyPartDestructible = "Destructible";
    [SerializeField] RagdollToggle ragdollToggle;
    [SerializeField] GameObject particleBlood;

    private void OnEnable()
    {
        SubcribeDetectDamanges();

        ragdollToggle.DesactiveRagdoll();
    }

    public void SubcribeDetectDamanges()
    {
        foreach (var bodyPart in ragdollToggle.GetBodyParts())
        {
            if (bodyPart.TryGetComponent<BodyPartDetectorDamange>(out BodyPartDetectorDamange detectorInBodyPart))
            {
                detectorInBodyPart.DetectDamange += OnDamageDetected;
                detectorInBodyPart.tagDetector = tagDetector;
            }
            else
            {
                bodyPart.AddComponent<BodyPartDetectorDamange>();
                BodyPartDetectorDamange newDetectorInBodyPar = bodyPart.GetComponent<BodyPartDetectorDamange>();

                newDetectorInBodyPar.DetectDamange += OnDamageDetected;
                newDetectorInBodyPar.tagDetector = tagDetector;
            }
        }
    }

    public void UnsubcribeDetectDamanges()
    {
        foreach (var bodyPart in ragdollToggle.GetBodyParts())
        {
            if (bodyPart.TryGetComponent<BodyPartDetectorDamange>(out BodyPartDetectorDamange detectorInBodyPart))
            {
                detectorInBodyPart.DetectDamange -= OnDamageDetected;
                detectorInBodyPart.tagDetector = tagDetector;
            }
        }
    }

    public void OnDamageDetected(GameObject bodyPart)
    {
        if (bodyPart.tag != tagBodyPartDestructible) return;

        if (TryGetComponent<Collider>(out Collider col)) col.enabled = false;

        if (bodyPart.transform.localScale == Vector3.zero) return;

        bodyPart.transform.localScale = Vector3.zero;

        // Particulas de sangre
        Instantiate(particleBlood, bodyPart.transform.position, bodyPart.transform.rotation);

        ragdollToggle.ActiveRagdoll();
    }
}
