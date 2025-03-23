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

    //CoyotimeTime es para que el jugador pueda saltar aunque ya haya abandonado el suelo
    private float coyoteTime=0.2f;
    private float coyoteTimeCounter;

    //JumpBuffering es para que el juego registre la acción de salto un poco antes de que el jugador toque el suelo
    private float jumpBufferTime=0.2f;
    private float jumpBufferCounter;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
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

        // Comprobar si está en el suelo con un Raycast
        //Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        //Grounded = Physics2D.Raycast(transform.position, Vector3.down, 0.05f);

        // Actualizar el estado de suelo en el Animator
        Animator.SetBool("isGrounded", Grounded);

        if (Grounded){
            coyoteTimeCounter=coyoteTime;
        } 
        else{
            coyoteTimeCounter-=Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.W)){
            jumpBufferCounter = jumpBufferTime;
        }else{
            jumpBufferCounter-=Time.deltaTime;
        }

        // Saltar si se presiona "W" y está en el suelo
		if (jumpBufferCounter >0f && coyoteTimeCounter > 0f)
        {
            Jump();
            jumpBufferCounter=0f;
            coyoteTimeCounter=0f;
        }
    }
	
    private void Jump()
    {
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
        if (colision.gameObject.CompareTag("suelo"))
        {
            Grounded = true;
        }
    }
}

