using UnityEngine;

public class Skeleton_Script : MonoBehaviour
{
    public GameObject Wizard;
    public float speed = 2.0f;
    private Animator anim; 

    void Start()
    {
        anim = GetComponent<Animator>(); 
    }

    void Update()
    {
        Vector3 direccion = Wizard.transform.position - transform.position;
        direccion.y = 0; // Evita movimiento vertical

        // Si esta lo suficientemente cerca, moverse
        if (direccion.magnitude < 10.0f)
        {
            transform.position += direccion.normalized * speed * Time.deltaTime;
            anim.SetFloat("Speed", speed); // Cambia animación a "Walk"
        }
        else
        {
            anim.SetFloat("Speed", 0); // Cambia animación a "Idle"
        }

        // Voltear sprite si es necesario
        if (direccion.x >= 0.0f)
            transform.localScale = new Vector3(4.0f, 4.0f, 4.0f);
        else
            transform.localScale = new Vector3(-4.0f, 4.0f, 4.0f);
    }
}
