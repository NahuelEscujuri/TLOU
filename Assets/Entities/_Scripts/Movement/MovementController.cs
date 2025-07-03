using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour, IMovement
{
    public event Action OnMove;

    [SerializeField] protected float walkSpeed = 2;
    [SerializeField] protected float runSpeed = 5;
    [SerializeField] protected float sprintMultiplier = 1.5f; // Modifica este valor según lo que consideres un "leve" sprint.
    [SerializeField] protected float sprintDuration = 0.5f;
    [SerializeField] protected bool canMove = true;
    [SerializeField] protected Rigidbody rb;
    protected Vector2 vMove;

    private void Awake() => rb = GetComponent<Rigidbody>();

    public bool IsMoving() => vMove != Vector2.zero;
    public float SpeedVelocity() => (IsRun() ? runSpeed : walkSpeed);
    public bool IsRun() => Input.GetAxis("Fire3") == 1;
    public void ChangeMoveState(bool value)=> canMove = value;

    private void Update()
    {
        if(!canMove)
        {
            vMove = Vector2.zero;
        }
        else
        {
            vMove.x = Input.GetAxis("Horizontal");
            vMove.y = Input.GetAxis("Vertical");
        }
    }

    #region metodos de movimiento
    public void StartMoveToTarget(Vector3 target, float distance, float duration)
    {
        StartCoroutine(MoveToTarget(target, distance, duration));
    }

    public IEnumerator MoveToTarget(Vector3 target, float distance, float duration)
    {
        Vector3 direccion = (transform.position - target);
        direccion.y = 0; // Eliminamos la componente vertical
        direccion = direccion.normalized;

        Vector3 destino = target + direccion * distance;
        destino.y = transform.position.y; // Conservamos la altura original

        Vector3 posicionInicial = transform.position;
        float tiempo = 0f;

        while (tiempo < duration)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.Clamp01(tiempo / duration);
            Vector3 nuevaPosicion = Vector3.Lerp(posicionInicial, destino, t);
            nuevaPosicion.y = transform.position.y; // Seguimos conservando la altura
            transform.position = nuevaPosicion;
            yield return null;
        }

        // Asegura que la posición final sea exacta
        transform.position = destino;
    }
    #endregion


    /// Activa un sprint en la dirección especificada.
    public void Sprint(Vector3 moveDirection)
    {
        StartCoroutine(SprintCoroutine(moveDirection));
    }

    /// Corrutina que aplica un aumento de velocidad temporal en la dirección de sprint.
    private IEnumerator SprintCoroutine(Vector3 normalizedDirection)
    {
        // Se obtiene la velocidad base actual
        Vector3 originalVelocity = rb.velocity;
        // Se define la velocidad de sprint
        Vector3 sprintVelocity = normalizedDirection * SpeedVelocity() * sprintMultiplier;

        // Duración para acelerar y desacelerar (se divide en dos)
        float halfDuration = sprintDuration / 2f;
        float elapsed = 0f;

        // Fase de aceleración: de la velocidad normal a la de sprint.
        while (elapsed < halfDuration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / halfDuration);
            rb.velocity = Vector3.Lerp(originalVelocity, sprintVelocity, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        // Aseguramos que se alcanza la velocidad de sprint.
        rb.velocity = sprintVelocity;

        // Reiniciamos el tiempo para la fase de desaceleración.
        elapsed = 0f;
        // Fase de desaceleración: de la velocidad de sprint de regreso a la velocidad original.
        while (elapsed < halfDuration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / halfDuration);
            rb.velocity = Vector3.Lerp(sprintVelocity, originalVelocity, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        // Se restablece la velocidad original.
        rb.velocity = originalVelocity;
    }
}