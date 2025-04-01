using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public GameObject Wizard;
    public Camera camara; // Referencia a la cámara
    public float suavizado = 5f; // Velocidad de seguimiento
    private float alturaExtra = 0f; // Altura adicional de la cámara
    private float zoomExtra = 0f; // Zoom adicional (tamaño extra de la cámara)
    private bool enTransicion = false;

    void Update()
    {
        Vector3 posicionObjetivo = transform.position;
        posicionObjetivo.x = Wizard.transform.position.x; // La cámara sigue al jugador en X
        posicionObjetivo.y = Wizard.transform.position.y + alturaExtra; // Mantiene la altura extra

        // Aplicar movimiento suave a la posición y zoom
        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, Time.deltaTime * suavizado);
        camara.orthographicSize = Mathf.Lerp(camara.orthographicSize, 5f + zoomExtra, Time.deltaTime * suavizado);
    }

    public void AjustarAlturaYZoom(float nuevaAltura, float nuevoZoom, float duracion)
    {
        if (!enTransicion)
        {
            StartCoroutine(TransicionAlturaYZoom(nuevaAltura, nuevoZoom, duracion));
        }
    }

    IEnumerator TransicionAlturaYZoom(float nuevaAltura, float nuevoZoom, float duracion)
    {
        enTransicion = true;
        float alturaInicial = alturaExtra;
        float zoomInicial = zoomExtra;
        float tiempo = 0;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            alturaExtra = Mathf.Lerp(alturaInicial, nuevaAltura, tiempo / duracion);
            zoomExtra = Mathf.Lerp(zoomInicial, nuevoZoom, tiempo / duracion);
            yield return null;
        }

        alturaExtra = nuevaAltura;
        zoomExtra = nuevoZoom;
        enTransicion = false;
    }
}
