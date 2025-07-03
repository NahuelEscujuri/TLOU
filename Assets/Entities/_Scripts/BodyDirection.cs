using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyDirection : MonoBehaviour
{
    public Vector2 CurrentVelocity => new(currentHorizontalSmooth, currentVerticalSmooth);
    public Transform GetBody => m_Body;

    [SerializeField] Transform m_Body;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float smoothSpeed = 5f; // Ajusta este valor para controlar la suavidad
    protected float currentHorizontalSmooth = 0f;
    protected float currentVerticalSmooth = 0f;
    IMovement movement;
    Rigidbody rb;
    Transform redirectTransform;
    Coroutine RedirectToTargetY;

    #region unity lifecycle
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<IMovement>();
    }
    private void Update()
    {
        if (movement.IsMoving()) Redirect();
        UpdateLocalMovement();
    }
    void Redirect()
    {
        Vector3 cameraForward = redirectTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
        m_Body.rotation = Quaternion.Slerp(m_Body.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    void UpdateLocalMovement()
    {
        Vector3 localVelocity = m_Body.InverseTransformDirection(rb.velocity);
        float maxSpeed = movement.SpeedVelocity();
        float normalizedHorizontal = Mathf.Clamp(localVelocity.x / maxSpeed, -1f, 1f);
        float normalizedVertical = Mathf.Clamp(localVelocity.z / maxSpeed, -1f, 1f);

        currentHorizontalSmooth = Mathf.Lerp(currentHorizontalSmooth, normalizedHorizontal, Time.deltaTime * smoothSpeed);
        currentVerticalSmooth = Mathf.Lerp(currentVerticalSmooth, normalizedVertical, Time.deltaTime * smoothSpeed);
    }
    #endregion
    public void ChangeRedirectTransform(Transform transform) => redirectTransform = transform;
   
    #region redirect coroutine
    public void StartRedirectToTransform(Transform target, float duration = .3f)
    {
        if(RedirectToTargetY != null) StopCoroutine(RedirectToTargetY);
        if(target) RedirectToTargetY = StartCoroutine(RedirectToTransform(m_Body.transform, target, duration));
    }
    IEnumerator RedirectToTransform(Transform source, Transform target, float duration)
    {
        // Calculamos la dirección plana (ignoramos Y)
        Vector3 dir = target.position - source.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f)
            yield break;

        // Rotación inicial y rotación objetivo
        Quaternion initialRot = source.rotation;
        Quaternion targetRot = Quaternion.LookRotation(dir);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            // Normalizamos t de 0 a 1
            float t = elapsed / duration;

            // Interpolamos suavemente la rotación en Y
            source.rotation = Quaternion.Slerp(initialRot, targetRot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Aseguramos rotación final exacta
        source.rotation = targetRot;
    }
    #endregion
}
