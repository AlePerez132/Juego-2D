using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Skeleton_Script : MonoBehaviour
{
    public GameObject Wizard;
    public float speed = 2.6f;
    public float alturaPermitida = 2.0f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckDistance = 3.0f;
    private Animator anim;
    public int vida = 20;
    private float tiempoUltimoAtaque = 0f;
    public float tiempoEntreAtaques = 1.5f;
    private bool estaMuerto = false;
     public Image barraVida;


    AudioManager audioManager;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (Wizard == null)
        {
            GameObject wizardEncontrado = GameObject.FindWithTag("Wizard");
            if (wizardEncontrado != null)
            {
                Wizard = wizardEncontrado;
            }
        }
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private bool HaySueloDelante()
    {
        if (groundCheck == null)
        {
            return false;
        }

        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        return hit != null;
    }

   void Update()
{
    if (Wizard == null || estaMuerto) return; 
    Vector3 direccion = Wizard.transform.position - transform.position;
    float distancia = direccion.magnitude;
    float diferenciaAltura = Mathf.Abs(Wizard.transform.position.y - transform.position.y);
    anim.SetFloat("Distance", distancia);

    if (direccion.x >= 0.0f) //Mirar hacia el mago
        transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
    else
        transform.localScale = new Vector3(-3.0f, 3.0f, 3.0f);

    if (distancia < 15.0f && HaySueloDelante()) //Sólo actúa si está dentro del rango de detección
    {
        //Ataque
        if (distancia < 1.8f && Time.time - tiempoUltimoAtaque > tiempoEntreAtaques && diferenciaAltura <= alturaPermitida)
        {
            anim.SetFloat("Speed", 0);
            anim.SetTrigger("Attack");
            tiempoUltimoAtaque = Time.time;
            
                if (!audioManager.SFXSource.isPlaying)
                {
                    audioManager.reproducirEfecto(audioManager.espadazo);
                }
        } else if (diferenciaAltura <= alturaPermitida && distancia < 15.0f) /*PERSECUCIÓN*/  {
            //Solo se mueve si está a la misma altura que el mago
            if (Mathf.Abs(Wizard.transform.position.y - transform.position.y) <= alturaPermitida) {
                anim.SetFloat("Speed", speed);
            } else {
                anim.SetFloat("Speed", 0);
            }
            // Moverse hacia el mago
            direccion.y = 0;
            transform.position += direccion.normalized * speed * Time.deltaTime;
        } else if (diferenciaAltura > alturaPermitida && distancia < 15.0f) {
            direccion.y = 0;
            transform.position += direccion.normalized * speed * Time.deltaTime;
            anim.SetFloat("Speed", speed);
        } else {
            anim.SetFloat("Speed", 0);
        }
    } else {
        anim.SetFloat("Speed", 0); //Fuera de rango de detección
    }
}


   public void HacerDanio()
{
    if (estaMuerto) return;

    if (Wizard != null)
    {
        WizardMovement wizardScript = Wizard.GetComponent<WizardMovement>();
        if (wizardScript != null)
        {
            wizardScript.RecibirDanio(8);
        }
    }
}


public void RecibirDanio(int cantidad)
{
    vida -= cantidad;

    if (barraVida != null)
    {
        barraVida.fillAmount = (float)vida / 20f;

        if (vida > 15)
            barraVida.color = Color.green;
        else if (vida > 10)
            barraVida.color = Color.yellow;
        else if (vida > 6)
            barraVida.color = new Color(1f, 0.647f, 0f); // Naranja claro
        else
            barraVida.color = Color.red;
    }

    if (vida <= 0)
    {
        estaMuerto = true;
        GetComponent<Collider2D>().enabled = false;
        Morir();
    }
    else
    {
        audioManager.reproducirEfecto(audioManager.esqueletoRecibirDanio);
    }
}


    void Morir()
{
    anim.SetTrigger("die");
    audioManager.reproducirEfecto(audioManager.muerteEnemigo);
    Destroy(gameObject, 0.8f);
}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("fireball"))
        {
            RecibirDanio(5);
            Destroy(collision.gameObject);
            Debug.Log("Esqueleto recibió daño!");
        }
    }

}

