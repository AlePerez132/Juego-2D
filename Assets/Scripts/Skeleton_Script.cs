using UnityEngine;

public class Skeleton_Script : MonoBehaviour
{
    public GameObject Wizard;
    public float speed = 2.7f;
    public float alturaPermitida = 2.0f; // Máxima diferencia de altura permitida para atacar
    public LayerMask groundLayer; // Capa del suelo
    public Transform groundCheck;
    public float groundCheckDistance = 3.0f;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>(); 
    }

    // Método que detecta si hay suelo delante
    private bool HaySueloDelante()
    {
        if (groundCheck == null)
        {
            // Debug.LogError("groundCheck no está asignado en el Inspector.");
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
        float diferenciaAltura = Mathf.Abs(Wizard.transform.position.y - transform.position.y); // Diferencia en altura

        anim.SetFloat("Distance", distancia);

        // Verifica si la diferencia de altura es aceptable antes de atacar
        if (distancia <= 1.8f && diferenciaAltura <= alturaPermitida)
        {
            anim.SetTrigger("Attack"); // Iniciar ataque
        }
        else if (distancia < 18.0f && HaySueloDelante()) //&& diferenciaAltura <= alturaPermitida)
        {
            // Si está en rango y hay suelo, caminar hacia el jugador
            direccion.y = 0; // Evita movimiento vertical
            transform.position += direccion.normalized * speed * Time.deltaTime;
            anim.SetFloat("Speed", speed); // Cambia animación a "Walk"
        }
        else
        {
            // Si está fuera del rango o en otra altura, quedarse quieto
            anim.SetFloat("Speed", 0); // Cambia animación a "Idle"
        }

        // Voltear sprite si es necesario
        if (direccion.x >= 0.0f)
            transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        else
            transform.localScale = new Vector3(-3.0f, 3.0f, 3.0f);
    }
}
