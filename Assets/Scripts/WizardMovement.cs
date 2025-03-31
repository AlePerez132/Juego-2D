using UnityEngine;
using UnityEngine.UI;

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
        if (isDead) return; // Si el personaje está muerto, no se ejecuta el código de movimiento

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

        if (Input.GetKey(KeyCode.C) && Time.time > LastShoot + 0.75f) {
            Shoot();
            LastShoot = Time.time;
        }
    }

    private void Shoot() { //DISPARO

        Vector3 direction;

        if(transform.localScale.x == 1.0f ) {
            direction = Vector2.right; 
        } else {
            direction = Vector2.left;
        }


    GameObject fireball_shot = Instantiate(fireball, transform.position + direction * 0.1f, Quaternion.identity);
    fireball_shot.GetComponent<fireball>().SetDirection(direction); //Del script de la bala
    }

    private void Jump() //SALTO
    {
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
        if (isDead) return; // Si está muerto, no recibe daño

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

        // Actualizar la barra de vida
        barraVida.fillAmount = (float)vidaActual / maxVida;

        // Cambiar color según la vida actual
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

        // Llamar a FinalizarMuerte después de 3 segundos 
        Invoke("FinalizarMuerte", 3f);
    }

    void FinalizarMuerte()
    {
        // Se desactiva el control del personaje y se detiene el movimiento
        this.enabled = false;  // Desactivar el script para que no siga moviéndose
        Rigidbody2D.linearVelocity = Vector2.zero;  // Detener el movimiento
        Rigidbody2D.isKinematic = true;  // Evitar que siga afectado por la gravedad
        DesactivarCollider();
        Animator.enabled = false;  // Desactivar el Animator para que no siga reproduciendo animaciones

        // Cerrar el juego tras 3 segundos
        Invoke("SalirDelJuego", 3f);
    }

    void DesactivarCollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    void SalirDelJuego()
    {
        Application.Quit(); // Cierra el juego
    }
}
