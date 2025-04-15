using UnityEngine;
using UnityEngine.SceneManagement;

public class FinNivel : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        //Comparamos la etiqueta del objeto con el que colisionamos
        if (other.gameObject.CompareTag("Player"))
        
    {
            // Guardamos el tiempo final
            PlayerPrefs.SetInt("TiempoFinal", Contador.instancia.segundosJugados);

            // Cargamos la escena de introducción de nombre
            SceneManager.LoadScene("Escena3");
        }
    }
}
