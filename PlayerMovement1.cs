using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    [Header("Movement")]
    public float Speed;
    public float JumpForce;
    public float airMultiplier;

    Vector3 moveDir;

    [Header("Inputs")]
    public KeyCode JumpKey = KeyCode.Space;
    
    float horiz;
    float vert;

    [Header("Drag")]
    public float groundDrag;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask Ground;
    public Transform Feet;

    [Header("Boolean Values")]
    public bool isGrounded;
    public bool isAir;

    [Header("Player Vars")]
    public Transform orientation;
    Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(Feet.position, 0.1f, Ground);

        PlayerInputs();
        DragCont();
        VelocityCont();
    }

    private void FixedUpdate()
    {
        Movement(); 
    }

    private void Movement()
    {
        if (isGrounded)
            rb.AddForce(moveDir.normalized * Speed * 10f, ForceMode.Force);
        else if (!isGrounded)
            rb.AddForce(moveDir.normalized * Speed * 10f * airMultiplier, ForceMode.Force);
    }

    private void PlayerInputs()
    {
        moveDir = orientation.forward * vert + orientation.right * horiz;

        horiz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(JumpKey) && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
        rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }

    private void DragCont()
    {
        if(isGrounded)
            rb.drag = groundDrag;
        else if (!isGrounded)
            rb.drag = 0.1f;
    }

    private void VelocityCont()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > Speed)
        {
            Vector3 limitedVel = flatVel.normalized * Speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
