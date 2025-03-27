using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardMovement : MonoBehaviour
{
    public float velocidad = 5f;
    public float JumpForce = 150f;

    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    private bool Grounded = false;
    private Animator Animator;

    private int groundCollisionCount = 0;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Screen.fullScreen = true;
    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");

        // Girar personaje según dirección
        if (Horizontal < 0.0f)
            transform.localScale = new Vector3(-3.0f, 3.0f, 1.0f);
        else if (Horizontal > 0.0f)
            transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);

        // Animaciones
        Animator.SetBool("running", Horizontal != 0.0f);
        Animator.SetBool("isGrounded", Grounded);

        if (Grounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Saltar
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            Jump();
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
    }

    private void Jump()
    {
        Rigidbody2D.linearVelocity = new Vector2(Rigidbody2D.linearVelocity.x, 0); 
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
        Grounded = false;
        Animator.SetTrigger("Jump");
    }

    void FixedUpdate()
    {
        Rigidbody2D.linearVelocity = new Vector2(Horizontal * velocidad, Rigidbody2D.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D colision)
    {
        if (colision.gameObject.CompareTag("suelo") || colision.gameObject.CompareTag("plataformas"))
        {
            groundCollisionCount++;
            Grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D colision)
    {
        if (colision.gameObject.CompareTag("suelo") || colision.gameObject.CompareTag("plataformas"))
        {
            groundCollisionCount--;
            if (groundCollisionCount <= 0)
            {
                Grounded = false;
                groundCollisionCount = 0; 
            }
        }
    }
}
