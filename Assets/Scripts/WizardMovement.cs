using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WizardMovement : MonoBehaviour
{
    public GameObject fireball;
    public GameObject gameOverMenu;
    public GameObject camara;
    public float velocidad = 5f;
    public float JumpForce = 150f;
    public float saltodanio = 500f;

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

    public int maxVida = 400;
    private int vidaActual;
    public Image barraVida;

    public bool isDead = false;
    private bool isInWater = false;

    AudioManager audioManager;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        if (!PlayerPrefs.HasKey("vidaMax"))
        {
            maxVida = 400;
        }
        else
        {
            maxVida = PlayerPrefs.GetInt("vidaMax");
        }
        if (PlayerPrefs.HasKey("vidaActual"))
        {
            vidaActual = PlayerPrefs.GetInt("vidaActual");
            barraVida.fillAmount = (float)vidaActual / maxVida;

            if (vidaActual > maxVida * 0.75f)
            {
                barraVida.color = Color.green;
            }
            else if (vidaActual > maxVida * 0.5f && vidaActual <= maxVida * 0.75f)
            {
                barraVida.color = Color.yellow;

            }
            else if (vidaActual > maxVida * 0.3f && vidaActual <= maxVida * 0.5f)
            {
                barraVida.color = new Color(1f, 0.647f, 0f);
            }
            else
            {
                barraVida.color = Color.red;
            }
        }
        else
        {
            vidaActual = maxVida;
        }

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
            //Esto es para que no salte si no hay suelo
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }


        if (jumpBufferCounter > 0f && (coyoteTimeCounter > 0f || isInWater))
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
            Invoke(nameof(Shoot), 0.2f);
            LastShoot = Time.time;
            if (Grounded)
                Animator.SetTrigger("finsalto");
        }
    }

    private void Shoot()
    {
        if (fireball == null)
        {
            Debug.LogError("Error: No hay prefab asignado en el Inspector.");
            return;
        }

        Vector3 direction = transform.localScale.x > 0.0f ? Vector2.right : Vector2.left;

        GameObject fireball_shot = Instantiate(fireball, transform.position + direction * 0.2f, Quaternion.identity);
        fireball_shot.GetComponent<Fireball>().SetDirection(direction);
    }

    private void Jump()
    {

        if (Input.GetKey(KeyCode.Space) && Time.time > LastShoot + 0.75f)
        {
            Shoot();
            LastShoot = Time.time;
        }

        if (isInWater)
        {
            Rigidbody2D.linearVelocity = new Vector2(Rigidbody2D.linearVelocity.x, 0);
            Rigidbody2D.AddForce(Vector2.up * JumpForce * 0.8f);
          
        }
        else
        {
            Rigidbody2D.linearVelocity = new Vector2(Rigidbody2D.linearVelocity.x, 0);
            Rigidbody2D.AddForce(Vector2.up * JumpForce);
        }

        Grounded = false;
        Animator.SetTrigger("Jump");
        audioManager.reproducirEfecto(audioManager.salto);
    }

    void FixedUpdate()
    {
        if (isDead) return;

        Rigidbody2D.linearVelocity = new Vector2(Horizontal * velocidad, Rigidbody2D.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D colision)
    {
        if (isDead) return;

        switch (colision.gameObject.tag)
        {
            case "suelo":
            case "limite":
            case "plataformas":
                groundCollisionCount++;
                Grounded = true;
                break;
            case "skeleton":
                RecibirDanio(15);
                break;
            case "peligro":
                RecibirDanio(10);
                saltoDeDanio();
                break;
            case "agua":
                isInWater = true;
                StartCoroutine(RecibirDanioAgua());
                break;
        }
    }

    void OnCollisionExit2D(Collision2D colision)
    {
        if (colision.gameObject.CompareTag("suelo") || colision.gameObject.CompareTag("plataformas") || colision.gameObject.CompareTag("agua"))
        {
            groundCollisionCount--;
            if (groundCollisionCount <= 0)
            {
                Grounded = false;
                groundCollisionCount = 0;
            }

            if (colision.gameObject.CompareTag("agua"))
            {
                isInWater = false;
            }
        }
    }

    private IEnumerator RecibirDanioAgua()
    {
        while (isInWater)
        {
            RecibirDanio(4);
            yield return new WaitForSeconds(0.5f);
            audioManager.reproducirEfecto(audioManager.recibirDanio);
        }
    }

    private void saltoDeDanio()
    {
        Rigidbody2D.linearVelocity = new Vector2(Rigidbody2D.linearVelocity.x, 0);
        Rigidbody2D.AddForce(Vector2.up * saltodanio * 3);
        Grounded = false;
        Animator.SetTrigger("Jump");
    }

    public void RecibirDanio(int cantidad)
    {
        if (isDead) return;

        vidaActual -= cantidad;
        Animator.SetTrigger("danio");
        vidaActual = Mathf.Clamp(vidaActual, 0, maxVida); // Para que la vida no sea negativa
        PlayerPrefs.SetInt("vidaActual", vidaActual);

        barraVida.fillAmount = (float)vidaActual / maxVida;

        if (vidaActual > maxVida * 0.75f)
        {
            barraVida.color = Color.green;
        }
        else if (vidaActual > maxVida * 0.5f && vidaActual <= maxVida * 0.75f)
        {
            barraVida.color = Color.yellow;

        }
        else if (vidaActual > maxVida * 0.3f && vidaActual <= maxVida * 0.5f)
        {
            barraVida.color = new Color(1f, 0.647f, 0f);
        }
        else
        {
            barraVida.color = Color.red;
        }

        if (vidaActual <= 0)
        {
            Morir();
        }
        else
        {
            audioManager.reproducirEfecto(audioManager.recibirDanio);
        }
    }

    void Morir()
    {
        isDead = true;
        audioManager.reproducirEfecto(audioManager.muerteMago);
        Animator.SetTrigger("muerte");
        Invoke("FinalizarMuerte", 2f);
    }

    void FinalizarMuerte()
    {
        this.enabled = false;
        Rigidbody2D.linearVelocity = Vector2.zero;
        Rigidbody2D.isKinematic = true;
        DesactivarCollider();
        Animator.enabled = false;
        ShowGameOverMenu();

        //Invoke("SalirDelJuego", 3f);
    }

    void DesactivarCollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    void SalirDelJuego()
    {
        Application.Quit();
    }

    void ShowGameOverMenu()
    {
        // Mover el menú a la posición de la cámara
        Debug.Log("LLamando al menu game over");
        Vector3 cameraPosition = camara.transform.position;
        gameOverMenu.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, gameOverMenu.transform.position.z);

    }
}
