using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Transform del hueso (bone) de la cabeza")]
    public Transform headTransform;
    [Tooltip("Cámara que sigue al personaje")]
    public Camera playerCamera;

    [Header("Ajustes")]
    [Tooltip("Distancia fija para proyectar el punto de mira si no hay colisión")]
    public float defaultAimDistance = 100f;
    [Tooltip("Velocidad de rotación (suavizado)")]
    [Range(0f, 20f)]
    public float rotateSpeed = 10f;

    [Header("Filtrado de capas")]
    [Tooltip("Selecciona las capas que quieres IGNORAR al hacer el raycast")]
    public LayerMask ignoreLayers;

    void LateUpdate()
    {
        if (headTransform == null || playerCamera == null)
            return;

        // 1) Rayo desde el centro de la cámara
        Ray centerRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        // 2) Raycast ignorando las capas indicadas
        Vector3 targetPoint;
        RaycastHit hit;
        int mask = ~ignoreLayers.value;
        // con ~ invertimos el mask para que incluya todas menos las que hemos marcado
        if (Physics.Raycast(centerRay, out hit, defaultAimDistance, mask))
        {
            targetPoint = hit.point;
        }
        else
        {
            // Si no colisiona con nada relevante, apuntamos a una distancia fija
            targetPoint = centerRay.origin + centerRay.direction * defaultAimDistance;
        }

        // 3) Cálculo de la rotación deseada
        Quaternion lookRotation = Quaternion.LookRotation(targetPoint - headTransform.position);

        // 4) Aplicamos suavizado
        headTransform.rotation = Quaternion.Slerp(
            headTransform.rotation,
            lookRotation,
            Time.deltaTime * rotateSpeed
        );
    }
}
