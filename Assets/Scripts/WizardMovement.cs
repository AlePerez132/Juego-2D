using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardMovement : MonoBehaviour
{
    public float velocidad = 5f;
    public float JumpForce = 150f;
    public float DoubleJumpForce = 120f; // Fuerza del doble salto
    
    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    private bool Grounded = false;
    private Animator Animator;
    
    private bool canSecondJump = false; // Para controlar el doble salto
    private bool firstJumpDone = false; // Para saber si ya se hizo el primer salto
    private bool canDoubleJump = false; // Controlar si se puede hacer el doble salto

    // CoyotimeTime es para que el jugador pueda saltar aunque ya haya abandonado el suelo
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    // JumpBuffering para que el juego registre la acción de salto un poco antes de que el jugador toque el suelo
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

        // Girar el personaje según la dirección
        if (Horizontal < 0.0f)
            transform.localScale = new Vector3(-3.0f, 3.0f, 1.0f);
        else if (Horizontal > 0.0f)
            transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);

        // Actualizar animación de correr
        Animator.SetBool("running", Horizontal != 0.0f);

        // Actualizar el estado de suelo en el Animator
        Animator.SetBool("isGrounded", Grounded);

        if (Grounded)
        {
            coyoteTimeCounter = coyoteTime;
            firstJumpDone = false; // Reseteamos la bandera para el siguiente salto
            canDoubleJump = false; // El doble salto no está disponible cuando estamos en el suelo
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

        // Comprobar si se puede saltar con la tecla "W" (primer salto)
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !firstJumpDone)
        {
            Jump();
            firstJumpDone = true; // Marcamos que ya se hizo el primer salto
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }

        // Comprobar si "J" habilita el doble salto (se activa al presionar "J" cuando ya estamos en el aire)
        if (Input.GetKeyDown(KeyCode.J) && !Grounded && !canDoubleJump)
        {
            canDoubleJump = true; // Habilitamos el doble salto
        }

        // Verificar el doble salto después de presionar "J" y estar en el aire
        if (Input.GetKeyDown(KeyCode.W) && canDoubleJump && firstJumpDone)
        {
            DoubleJump();
            canDoubleJump = false; // Deshabilitamos el doble salto después de hacerlo
        }
    }

    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
        Grounded = false;
        Animator.SetTrigger("Jump");
    }

    private void DoubleJump()
    {
        Rigidbody2D.linearVelocity = new Vector2(Rigidbody2D.linearVelocity.x, 0); // Resetear la velocidad vertical
        Rigidbody2D.AddForce(Vector2.up * DoubleJumpForce);
        Animator.SetTrigger("Jump");
    }

    void FixedUpdate()
    {
        Rigidbody2D.linearVelocity = new Vector2(Horizontal * velocidad, Rigidbody2D.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D colision)
    {
        if (colision.gameObject.CompareTag("suelo"))
        {
            Grounded = true;
        }
    }
}
