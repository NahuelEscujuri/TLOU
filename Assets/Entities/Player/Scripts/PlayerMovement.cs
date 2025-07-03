using System;
using UnityEngine;

public class PlayerMovement : MovementController, IMovement
{
    [SerializeField] Transform m_Body;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float groundCheckDistance = 0.2f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] bool useRb = false;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        MovePlayer();
        if (Input.GetButtonDown("Jump")) Sprint(new Vector3(vMove.x, 0, vMove.y));
    }

    void MovePlayer()
    {
        // Si no se puede mover
        if (!canMove) return;

        // Si est� en movimiento, invoca el evento
        //if (IsMoving()) OnMove?.Invoke();

        // Direcci�n base (en el espacio de la c�mara)
        Vector3 inputDirection = new Vector3(vMove.x, 0f, vMove.y).normalized;

        // Tomamos la rotaci�n de la c�mara y la aplicamos al input
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        // Ignoramos inclinaci�n vertical
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Convertimos el input en direcci�n del mundo usando la c�mara
        Vector3 moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;
        moveDirection = moveDirection.normalized * SpeedVelocity();

        // Aplicamos el movimiento
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
    }
}
