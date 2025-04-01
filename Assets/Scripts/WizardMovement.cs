using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WizardMovement : MonoBehaviour
{
    public GameObject fireball;
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
    private float LastShoot;

    public int maxVida = 200;
    private int vidaActual;
    public Image barraVida;

    private bool isDead = false; 

    AudioManager audioManager;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        vidaActual = maxVida;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (isDead) return; 

        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f)
            transform.localScale = new Vector3(-3.0f, 3.0f, 1.0f);
        else if (Horizontal > 0.0f)
            transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);

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

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            Jump();
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }

        if (Horizontal != 0.00f && Grounded && !audioManager.SFXSource.isPlaying)
        {
            audioManager.reproducirEfecto(audioManager.andar);
        }

        if (Input.GetKeyDown(KeyCode.C) && Time.time > LastShoot + 0.75f) 
 {    

            Animator.SetTrigger("ataque");
            Invoke(nameof(Shoot), 0.2f); // Llama a Shoot después de 1 segundo
            LastShoot = Time.time;

        }
    }

   private void Shoot() {
    if (fireball == null) {
        Debug.LogError("Error: No hay prefab asignado en el Inspector.");
        return;
    }

    Vector3 direction;  

    if (transform.localScale.x > 0.0f) {
        direction = Vector2.right;
    } else {
        direction = Vector2.left;
    }

    GameObject fireball_shot = Instantiate(fireball, transform.position + direction * 0.2f, Quaternion.identity);
    Debug.Log("Bala instanciada en: " + fireball_shot.transform.position);

    fireball_shot.GetComponent<Fireball>().SetDirection(direction);
}

    private void Jump() //SALTO
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > LastShoot + 0.75f) {
            Shoot();
            LastShoot = Time.time;
        }

        Rigidbody2D.linearVelocity = new Vector2(Rigidbody2D.linearVelocity.x, 0);
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
        Grounded = false;
        Animator.SetTrigger("Jump");
        audioManager.reproducirEfecto(audioManager.salto);
    }

    void FixedUpdate()
    {
        if (isDead) return; // Si está muerto, no actualizamos el movimiento

        Rigidbody2D.linearVelocity = new Vector2(Horizontal * velocidad, Rigidbody2D.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D colision)
    {
        if (isDead) return; 

        if (colision.gameObject.CompareTag("suelo") || colision.gameObject.CompareTag("plataformas"))
        {
            groundCollisionCount++;
            Grounded = true;
            //audioManager.reproducirEfecto(audioManager.caer); se ejecuta varias veces al andar, no entiendo por que
        }

        if (colision.gameObject.CompareTag("skeleton"))
        {
            RecibirDanio(15);
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

    public void RecibirDanio(int cantidad)
    {
        if (isDead) return;

        
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, maxVida); // Para que la vida no sea negativa

        //Actualiza la barra de vida
        barraVida.fillAmount = (float)vidaActual / maxVida;

        if (vidaActual > maxVida * 0.6f)
        {
            barraVida.color = Color.green;
        }
        else if (vidaActual > maxVida * 0.38f)
        {
            barraVida.color = Color.yellow;
        }
        else
        {
            barraVida.color = Color.red;
        }

        if (vidaActual <= 0)
        {
            Morir();
        } else 
        {
            audioManager.reproducirEfecto(audioManager.recibirDanio);
        }
    }


    void Morir()
    {
        isDead = true; 
        audioManager.reproducirEfecto(audioManager.muerteMago);
        Animator.SetTrigger("muerte");
        Invoke("FinalizarMuerte", 3f);
    }

    void FinalizarMuerte()
    {
        this.enabled = false;  
        Rigidbody2D.linearVelocity = Vector2.zero;  //Se detiene el movimiento
        Rigidbody2D.isKinematic = true;  //Evita que siga afectado por la gravedad
        DesactivarCollider();
        Animator.enabled = false; 

        Invoke("SalirDelJuego", 3f);
    }

    void DesactivarCollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    void SalirDelJuego()
    {
        Application.Quit(); 
    }
}
