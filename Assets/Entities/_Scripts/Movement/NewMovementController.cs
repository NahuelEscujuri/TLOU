using System;
using UnityEngine;

public class NewMovementController : MonoBehaviour, IMovement
{
    public bool canMove = true;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] Animation anim;
    [SerializeField] Rigidbody rb;
    [Header("Speed")]
    [SerializeField] float walkSpeed = 2;
    [SerializeField] float acceleration = .6f;
    [Header("Sprint")]
    [SerializeField] float sprintMultiplier = 1.5f;
    [SerializeField] float sprintDuration = 0.5f;
    [Header("Crouch")]
    [SerializeField, Range(0f, 1f)] float crouchHeightMultiplier = .6f;
    [SerializeField, Range(0f, 1f)] float crouchMultiplier = .6f;
    [Header("Ground")]
    [SerializeField] float groundCheckDistance = 0.2f;
    [SerializeField] LayerMask groundMask;
    float normalHeight;
    float crouchHeght;
    Vector3 vMove;
    Vector2 vInput;
    public float GetCurrentSpeed => currentSpeed;
    float currentSpeed;

    void Start()
    {
        normalHeight = capsuleCollider.height;
        crouchHeght = normalHeight * crouchHeightMultiplier;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        currentSpeed = walkSpeed;
    }
    private void Update() => UpdateVectorMove();
    void UpdateVectorMove()
    {
        if (!canMove)
        {
            vMove = Vector3.zero;
            return;
        }
        vInput.x = Input.GetAxis("Horizontal");
        vInput.y = Input.GetAxis("Vertical");      

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        vMove = cameraForward * vInput.y + cameraRight * vInput.x;
        vMove.Normalize();
    }

    void FixedUpdate() => Move();
    void Move()
    {
        float targetSpeed = !IsMoving() ? 0f : IsRun() ? walkSpeed * sprintMultiplier : IsCrouch() ? walkSpeed * crouchMultiplier : walkSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);

        if (IsCrouch())
        {
            anim.CrossFade("locomotion_crouch", .3f);
            capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, crouchHeght, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            anim.CrossFade("locomotion", .3f);
            capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, normalHeight, acceleration * Time.fixedDeltaTime);
        }
        Vector3 center = capsuleCollider.center;
        center.y = capsuleCollider.height * .5f;
        capsuleCollider.center = center;
        anim.CrossFade(IsCrouch() ? "locomotion_crouch" : "locomotion", .3f);

        rb.velocity = currentSpeed * (vMove.z * transform.forward + vMove.x * transform.right) + Vector3.up * rb.velocity.y;
    }

    public bool IsMoving() => vMove != Vector3.zero;
    public float SpeedVelocity() => currentSpeed;
    public bool IsRun() => Input.GetKey(KeyCode.LeftShift) && !IsCrouch();
    public bool IsCrouch() => Input.GetKey(KeyCode.LeftControl);
}