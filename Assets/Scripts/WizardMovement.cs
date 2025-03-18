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
    
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f) transform.localScale = new Vector3(-3.0f, 3.0f, 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);

        Animator.SetBool("running",Horizontal != 0.0f);

        if (Grounded){
        Debug.Log("Estoy tocando el suelo");
        }

        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.05f)) 
        {
            Grounded=true;
        }
        else
        {
            Grounded=false;
        }

		if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }
    }
	
    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

	void FixedUpdate() 
	{
		Rigidbody2D.linearVelocity = new Vector2(Horizontal*velocidad, Rigidbody2D.linearVelocity.y);
	
	}
}
