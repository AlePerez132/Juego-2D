using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float Speed;
    private Rigidbody2D Rigidbody2D;
    private Vector2 Direction;
    public int damage = 5;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Debug.Log("Bala creada en: " + transform.position);
    }


    void FixedUpdate()
    {
        Rigidbody2D.linearVelocity = Direction * Speed;
    }

    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
        Debug.Log("Dirección asignada a la bala: " + Direction);
    }
private void OnTriggerEnter2D(Collider2D collision)
{

    switch (collision.gameObject.tag)
    {
        case "suelo":
        case "limite":
        case "plataformas":
            Destroy(gameObject);
            break;
        case "skeleton":
            Skeleton_Script enemigo = collision.gameObject.GetComponent<Skeleton_Script>();
            if (enemigo != null)
            {
                enemigo.RecibirDanio(damage);
            }
            Destroy(gameObject);
            break;
        default:
            Debug.Log("Otro objeto detectado: " + collision.gameObject.tag);
            break;
    }
}

    public void DestroyFireball()
    {
        Destroy(gameObject, 3.5f);
    }
}
