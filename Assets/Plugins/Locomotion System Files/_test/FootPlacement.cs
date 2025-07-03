using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(RigBuilder))]
public class FootPlacement : MonoBehaviour
{
    [System.Serializable]
    public class Foot
    {
        public TwoBoneIKConstraint ikConstraint;
        public Transform target;        // el GameObject Target
        public float raycastHeight = 1; // altura desde la cadera
        public float maxDistance = 2f;
    }

    public Foot leftFoot;
    public Foot rightFoot;
    public LayerMask groundLayers;

    void LateUpdate()
    {
        UpdateFoot(leftFoot);
        UpdateFoot(rightFoot);
    }

    void UpdateFoot(Foot foot)
    {
        // Posición de origen para raycast: un poco arriba del tobillo
        Vector3 origin = foot.target.position + Vector3.up * foot.raycastHeight;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, foot.raycastHeight + foot.maxDistance, groundLayers))
        {
            // Mueve el target al punto de contacto
            foot.target.position = hit.point;
            // Alínea la rotación al normal del suelo
            foot.target.rotation = Quaternion.LookRotation(transform.forward, hit.normal);
            foot.ikConstraint.weight = 1f;
        }
        else
        {
            // Si no hay suelo cerca, desactiva IK
            foot.ikConstraint.weight = 0f;
        }
    }
}