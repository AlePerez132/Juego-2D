using UnityEngine;

public class plataformas : MonoBehaviour
{
    public float speed = 2f;      // Velocidad de movimiento
    public float height = 3f;     // Distancia que subirá
    private Vector3 startPos;     // Posición inicial

    private void Start()
    {
        startPos = transform.position; // Guarda la posición inicial
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;
        col.enabled = true;
    }
    

    private void Update()
    {
        float newY = startPos.y + Mathf.PingPong(Time.time * speed, height);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}

