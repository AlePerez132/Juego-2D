using UnityEngine;

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
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        
        Vector3 direccion = Wizard.transform.position - transform.position;
        float distancia = direccion.magnitude;
        float diferenciaAltura = Mathf.Abs(Wizard.transform.position.y - transform.position.y);

        anim.SetFloat("Distance", distancia);

        if (distancia <= 1.8f && diferenciaAltura <= alturaPermitida)
        {
            anim.SetTrigger("Attack");
        }
        else if (distancia < 15.0f && HaySueloDelante())
        {
            direccion.y = 0;
            transform.position += direccion.normalized * speed * Time.deltaTime;
            anim.SetFloat("Speed", speed);
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }

        if (direccion.x >= 0.0f)
            transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        else
            transform.localScale = new Vector3(-3.0f, 3.0f, 3.0f);
    }

    public void HacerDanio()
    {
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
        if (vida <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        anim.SetTrigger("die");
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

