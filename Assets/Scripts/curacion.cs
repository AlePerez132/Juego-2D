using UnityEngine;

public class curacion : MonoBehaviour
{
     public int cantidadCura = 20;
    public float floatAmplitude = 0.2f; // altura del movimiento
    public float floatSpeed = 2f; // velocidad del movimiento

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Movimiento flotante vertical
        float y = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPos.x, startPos.y + y, startPos.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        WizardMovement jugador = other.GetComponent<WizardMovement>();

        if (jugador != null && !jugador.isDead)
        {
            jugador.CurarVida(cantidadCura);
            //audioManager.reproducirEfecto(audioManager.recogerCura);
            Destroy(gameObject);
        }
    }
}
