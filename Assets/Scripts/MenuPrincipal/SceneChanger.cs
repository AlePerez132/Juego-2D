using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CambiarEscena : MonoBehaviour
{
    private string nombreEscena = "Juego"; // Nombre de la escena a cargar

    public void CambiarDeEscena()
    {
        StartCoroutine(CambiarEscenaDespuesDeSonido());
    }

    //es para que siempre se reproduzca el sonido antes de cargar la escena
    IEnumerator CambiarEscenaDespuesDeSonido()
    {
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(nombreEscena);
    }
}
