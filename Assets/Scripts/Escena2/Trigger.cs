using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour
{
    public Transform vallas;
    public Vector3 posicionObjetivo; // Posición final deseada
    public CameraScript camara;
    public float duracionMovimiento = 0.5f; // Tiempo que tarda en moverse

    private bool enMovimiento = false; // Evita que el movimiento se active varias veces

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !enMovimiento) // Solo mover si no está en movimiento
        {
            StartCoroutine(MoverObjeto(vallas, vallas.position, posicionObjetivo, duracionMovimiento));

            if (camara != null)
            {
                camara.AjustarAlturaYZoom(4f, 2f, 0.5f); //LUEGO HAY QUE HACER EL EFECTO INVERSO CUANDO NOS PASEMOS EL BOSS
            }
        }
    }

    IEnumerator MoverObjeto(Transform objeto, Vector3 inicio, Vector3 destino, float duracion)
    {
        enMovimiento = true; // Bloquea movimientos adicionales
        float tiempo = 0;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            objeto.position = Vector3.Lerp(inicio, destino, tiempo / duracion);
            yield return null; // Espera al siguiente frame
        }

        objeto.position = destino; // Asegura la posición final
        enMovimiento = false; // Permite otro movimiento si es necesario
    }
}
